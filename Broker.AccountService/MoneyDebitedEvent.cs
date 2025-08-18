using Broker.Entity;

namespace Broker.AccountService;

public class MoneyDebitedEvent : EventWrapper
{
    public MoneyDebitedEvent(Guid eventId, DateTime occurredAt, Metadata meta, string currency, string reason) : base(eventId, occurredAt, meta)
    {
        Currency = currency;
        Reason = reason;
    }

    public Guid OperationId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Reason { get; set; }
}