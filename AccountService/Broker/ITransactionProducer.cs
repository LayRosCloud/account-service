using Broker.AccountService;

namespace AccountService.Broker;

public interface ITransactionProducer : IProducer<MoneyDebitedEvent>, IProducer<MoneyCreditedEvent>
{
}