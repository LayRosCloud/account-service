using Broker.AccountService;
using Broker.Handlers;
using NotificationService.Features.Broker;
using RabbitMQ.Client;

namespace NotificationService.Features.Account;

public class AccountConsumer : IConsumer<AccountOpenedEvent>
{
    private readonly IEventRepository _repository;
    private readonly IDictionary<string, Func<AccountOpenedEvent, Task>> _actions;
    private readonly Dictionary<string, object?> _args = new()
    {
        { "x-dead-letter-exchange", "dlx" },
        { "x-dead-letter-routing-key", "errors" }
    };

    private AccountConsumer(IEventRepository repository)
    {
        _repository = repository;
        _actions = new Dictionary<string, Func<AccountOpenedEvent, Task>>
        {
            {"v1", Handle}
        };
    }

    private async Task InitializeAsync(IChannel channel)
    {
        await channel.ExchangeDeclareAsync("account.events", ExchangeType.Topic, durable: true);
        var queue = await channel.QueueDeclareAsync("account-service-queue", durable: true, arguments: _args);
        var queueName = queue.QueueName;
        await channel.QueueBindAsync(queueName, "account.events", "account.*");
    }

    public async Task ConsumeAsync(AccountOpenedEvent entity)
    {
        if (await _repository.ExistsEventAsync(entity.EventId))
        {
            return;
        }

        await _repository.CreateAsync(new EventEntity(entity.EventId, DateTime.UtcNow, "Account opened"));
        await _repository.SaveChangesAsync();
        await _actions[entity.Meta.Version].Invoke(entity);
    }

    private static Task Handle(AccountOpenedEvent entity)
    {
        Console.WriteLine($"Handle version V1: id: {entity.EventId}");
        return Task.CompletedTask;
    }

    public static async Task<IConsumer<AccountOpenedEvent>> CreateAsync(IEventRepository repository, IChannel channel)
    {
        var consumer = new AccountConsumer(repository);
        await consumer.InitializeAsync(channel);
        return consumer;
    }
}