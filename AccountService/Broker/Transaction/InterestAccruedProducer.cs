using Broker.AccountService;
using Broker.Handlers;

namespace AccountService.Broker.Transaction;

public class InterestAccruedProducer : IProducer<InterestAccruedEvent>
{
    private readonly IMessageProducer _producer;

    public InterestAccruedProducer(IMessageProducer producer)
    {
        _producer = producer;
    }

    public async Task ProduceAsync(InterestAccruedEvent param)
    {
        await _producer.SendAsync(param, "interest-accrued-service-queue", "interest.accrued.events", "interest.accrued.events");
    }
}