using Broker.AccountService;
using NotificationService.Features.Broker;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace NotificationService.Features.Transaction;

public class TransactionMessageConsumer
{
    private readonly IEventRepository _repository;
    private readonly IChannel _channel;
    private readonly IDictionary<string, Func<MoneyCreditedEvent, Task>> _creditedActions;
    private readonly IDictionary<string, Func<MoneyDebitedEvent, Task>> _debitedActions;
    private readonly IDictionary<string, Func<TransferCompletedEvent, Task>> _transferActions;
    private readonly Dictionary<string, object?> _args = new()
    {
        { "x-dead-letter-exchange", "dlx" },
        { "x-dead-letter-routing-key", "errors" }
    };

    private TransactionMessageConsumer(IEventRepository repository, IChannel channel)
    {
        _repository = repository;
        _channel = channel;
        _creditedActions = new Dictionary<string, Func<MoneyCreditedEvent, Task>>
        {
            {"v1", ConsumeAsync},
        };
        _debitedActions = new Dictionary<string, Func<MoneyDebitedEvent, Task>>
        {
            {"v1", ConsumeAsync},
        };
        _transferActions = new Dictionary<string, Func<TransferCompletedEvent, Task>>
        {
            {"v1", ConsumeAsync},
        };
    }

    private async Task InitializeAsync(IChannel channel)
    {
        await channel.ExchangeDeclareAsync("money.events", ExchangeType.Topic, durable: true);
        var queue = await channel.QueueDeclareAsync("money-service-queue", durable: true, arguments:_args);
        var queueName = queue.QueueName;
        await channel.QueueBindAsync(queueName, "money.events", "money.credited");
        await channel.QueueBindAsync(queueName, "money.events", "money.debited");
        await channel.QueueBindAsync(queueName, "money.events", "money.transfer.completed");
    }

    public Task ConsumeAsync(CancellationToken cancellationToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (sender, eventArgs) =>
        {
            try
            {
                await HandleAsync(sender, eventArgs, cancellationToken);
            }
            catch
            {
                await ((AsyncEventingBasicConsumer)sender).Channel.BasicNackAsync(eventArgs.DeliveryTag, false, false, cancellationToken);
            }
        };
        return Task.CompletedTask;
    }

    private async Task HandleAsync(object sender, BasicDeliverEventArgs eventArgs, CancellationToken cancellationToken)
    {
        var bodyBytes = eventArgs.Body.ToArray();
        var message = Encoding.UTF8.GetString(bodyBytes);
        switch (eventArgs.RoutingKey)
        {
            case "money.credited":
                var creditedMoney = JsonSerializer.Deserialize<MoneyCreditedEvent>(message);
                await _creditedActions[creditedMoney!.Meta.Version].Invoke(creditedMoney);
                break;
            case "money.debited":
                var debitedMoney = JsonSerializer.Deserialize<MoneyDebitedEvent>(message);
                await _debitedActions[debitedMoney!.Meta.Version].Invoke(debitedMoney);
                break;
            case "money.transfer.completed":
                var transfer = JsonSerializer.Deserialize<TransferCompletedEvent>(message);
                await _transferActions[transfer!.Meta.Version].Invoke(transfer);
                break;
            default:
                throw new InvalidOperationException("Invalid exception");
        }
        await ((AsyncEventingBasicConsumer)sender).Channel.BasicAckAsync(eventArgs.DeliveryTag, false, cancellationToken);
    }

    private async Task ConsumeAsync(MoneyCreditedEvent entity)
    {
        if (await _repository.ExistsEventAsync(entity.EventId))
        {
            return;
        }

        await _repository.CreateAsync(new EventEntity(entity.EventId, DateTime.UtcNow, "Credited Event"));
        await _repository.SaveChangesAsync();
        Console.WriteLine($"Handle credit event: {entity.EventId}");
    }

    private async Task ConsumeAsync(MoneyDebitedEvent entity)
    {
        if (await _repository.ExistsEventAsync(entity.EventId))
        {
            return;
        }

        await _repository.CreateAsync(new EventEntity(entity.EventId, DateTime.UtcNow, "Debited Event"));
        await _repository.SaveChangesAsync();
        Console.WriteLine($"Handle debit event: {entity.EventId}");
    }

    private async Task ConsumeAsync(TransferCompletedEvent entity)
    {
        if (await _repository.ExistsEventAsync(entity.EventId))
        {
            return;
        }

        await _repository.CreateAsync(new EventEntity(entity.EventId, DateTime.UtcNow, "Transfer Event"));
        await _repository.SaveChangesAsync();
        Console.WriteLine($"Handle transfer event: {entity.EventId}");
    }

    public static async Task<TransactionMessageConsumer> CreateAsync(IEventRepository repository, IConnection connection)
    {
        var channel = await connection.CreateChannelAsync();
        var res = new TransactionMessageConsumer(repository, channel);
        await res.InitializeAsync(channel);
        return res;
    }
}