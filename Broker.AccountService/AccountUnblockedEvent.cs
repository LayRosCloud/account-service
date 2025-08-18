using Broker.Entity;

namespace Broker.AccountService;

public class AccountUnblockedEvent : EventWrapper
{
    public AccountUnblockedEvent(Guid eventId, DateTime occurredAt, Metadata meta) : base(eventId, occurredAt, meta)
    {
    }

    public Guid ClientId { get; set; }
}