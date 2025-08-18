using Broker.Entity;

namespace Broker.AccountService;

public class AccountBlockedEvent : EventWrapper
{
    public AccountBlockedEvent(Guid eventId, DateTime occurredAt, Metadata meta) : base(eventId, occurredAt, meta)
    {

    }

    public Guid ClientId { get; set; }
}