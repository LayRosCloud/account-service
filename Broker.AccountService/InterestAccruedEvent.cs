using Broker.Entity;

namespace Broker.AccountService;

public class InterestAccruedEvent : EventWrapper
{
    public InterestAccruedEvent(Guid eventId, DateTime occurredAt, Metadata meta) : base(eventId, occurredAt, meta)
    {
    }

    public DateTime PeriodFrom { get; set; }
    public DateTime PeriodTo { get; set; }
    public decimal Amount { get; set; }
}