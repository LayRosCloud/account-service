using Broker.AccountService;
using Broker.Handlers;

namespace AccountService.Broker.Account;

public class AccountProducer : IProducer<AccountOpenedEvent>
{
    private readonly IMessageProducer _producer;

    public AccountProducer(IMessageProducer producer)
    {
        _producer = producer;
    }

    public async Task ProduceAsync(AccountOpenedEvent param)
    {
        await _producer.SendAsync(param, "account.events");
    }
}