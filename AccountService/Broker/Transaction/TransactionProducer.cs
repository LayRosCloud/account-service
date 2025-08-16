using Broker.AccountService;
using Broker.Handlers;

namespace AccountService.Broker.Transaction;

public class TransactionProducer : ITransactionProducer, IProducer<TransferCompletedEvent>
{
    private readonly IMessageProducer _producer;
    private const string ExchangeName = "money.events";

    public TransactionProducer(IMessageProducer producer)
    {
        _producer = producer;
    }

    public async Task ProduceAsync(MoneyCreditedEvent param)
    {
        await _producer.SendAsync(param, "money.credited", ExchangeName);
    }

    public async Task ProduceAsync(MoneyDebitedEvent param)
    {
        await _producer.SendAsync(param, "money.debited", ExchangeName);
    }

    public async Task ProduceAsync(TransferCompletedEvent param)
    {
        await _producer.SendAsync(param, "money.transfer.completed", ExchangeName);
    }
}