using Broker.Entity;

namespace Broker.AccountService;

public class MoneyCreditedEvent : EventWrapper
{
    public MoneyCreditedEvent(Guid eventId, DateTime occurredAt, Metadata meta, string currency) : base(eventId, occurredAt, meta)
    {
        Currency = currency;
    }

    public Guid OperationId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
}