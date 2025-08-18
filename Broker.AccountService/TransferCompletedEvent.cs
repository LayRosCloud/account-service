using Broker.Entity;

namespace Broker.AccountService;

public class TransferCompletedEvent : EventWrapper
{
    public TransferCompletedEvent(Guid eventId, DateTime occurredAt, Metadata meta, string currency) : base(eventId, occurredAt, meta)
    {
        Currency = currency;
    }

    public Guid SourceAccountId { get; set; }
    public Guid DestinationAccountId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public Guid TransferId { get; set; }
}