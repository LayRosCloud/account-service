using Broker.Entity;

namespace Broker.AccountService;

public class AccountOpenedEvent: EventWrapper
{
    public AccountOpenedEvent(Guid eventId, DateTime occurredAt, Metadata meta, string currency) : base(eventId, occurredAt, meta)
    {
        Currency = currency;
    }
    public Guid AccountId { get; set; }
    public Guid OwnerId { get; set; }
    public string Currency { get; set; }
    public AccountType Type { get; set; }
}