using System.Text;
using System.Text.Json;
using AccountService.Broker.Events;
using AccountService.Features.Accounts;
using Broker.AccountService;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AccountService.Broker.Client;

public class ClientConsumer
{
    private readonly IChannel _channel;
    private readonly IEventRepository _repository;
    private readonly IAccountRepository _accountRepository;
    private readonly Dictionary<string, Func<AccountBlockedEvent, CancellationToken, Task>> _blockedAccount;
    private readonly Dictionary<string, Func<AccountUnblockedEvent, CancellationToken, Task>> _unblockedAccount;

    private ClientConsumer(IChannel channel, IEventRepository repository, IAccountRepository accountRepository)
    {
        _channel = channel;
        _repository = repository;
        _accountRepository = accountRepository;
        _blockedAccount = new Dictionary<string, Func<AccountBlockedEvent, CancellationToken, Task>>()
        {
            {"v1", ConsumeAsync}
        };
        _unblockedAccount = new Dictionary<string, Func<AccountUnblockedEvent, CancellationToken, Task>>()
        {
            {"v1", ConsumeAsync}
        };
    }

    private async Task InitializeAsync()
    {
        await _channel.ExchangeDeclareAsync("client.events", ExchangeType.Topic, durable: true);
        var queue = await _channel.QueueDeclareAsync("client-service-queue", durable: true);
        var queueName = queue.QueueName;
        await _channel.QueueBindAsync(queueName, "client.events", "client.blocked");
        await _channel.QueueBindAsync(queueName, "client.events", "client.unblocked");
    }

    public Task ConsumeAsync(CancellationToken cancellationToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (sender, eventArgs) =>
        {
            var bodyBytes = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(bodyBytes);
            switch (eventArgs.RoutingKey)
            {
                case "client.blocked":
                    var blockedClient = JsonSerializer.Deserialize<AccountBlockedEvent>(message);
                    await _blockedAccount[blockedClient!.Meta.Version].Invoke(blockedClient, cancellationToken);
                    break;
                case "client.unblocked":
                    var unblockedClient = JsonSerializer.Deserialize<AccountUnblockedEvent>(message);
                    await _unblockedAccount[unblockedClient!.Meta.Version].Invoke(unblockedClient, cancellationToken);
                    break;
                default:
                    throw new InvalidOperationException();
            }
            await ((AsyncEventingBasicConsumer)sender).Channel.BasicAckAsync(eventArgs.DeliveryTag, false, cancellationToken);

        };
        return Task.CompletedTask;
    }

    private async Task ConsumeAsync(AccountBlockedEvent account, CancellationToken token)
    {
        if (await _repository.ExistsEventByIdAsync(account.EventId))
        {
            return;
        }

        await _repository.CreateAsync(new Event(account.EventId, DateTime.UtcNow, "Blocked account"));
        await _repository.SaveChangesAsync(token);
        var list = await _accountRepository.FindAllByOwnerIdAsync(account.ClientId);
        foreach (var item in list)
        {
            item.IsFrozen = true;
        }

        _accountRepository.UpdateRange(list);
        await _repository.SaveChangesAsync(token);
    }

    private async Task ConsumeAsync(AccountUnblockedEvent account, CancellationToken token)
    {
        if (await _repository.ExistsEventByIdAsync(account.EventId))
        {
            return;
        }

        await _repository.CreateAsync(new Event(account.EventId, DateTime.UtcNow, "Blocked account"));
        await _repository.SaveChangesAsync(token);
        var list = await _accountRepository.FindAllByOwnerIdAsync(account.ClientId);
        foreach (var item in list)
        {
            item.IsFrozen = false;
        }

        _accountRepository.UpdateRange(list);
        await _repository.SaveChangesAsync(token);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.DisposeAsync();
    }

    public static async Task<ClientConsumer> CreateAsync(IChannel channel, IEventRepository repository, IAccountRepository accountRepository)
    {
        var consume = new ClientConsumer(channel, repository, accountRepository);
        await consume.InitializeAsync();
        return consume;
    }
}