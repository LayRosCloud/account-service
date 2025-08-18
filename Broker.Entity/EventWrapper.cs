namespace Broker.Entity;

public class EventWrapper
{
    public EventWrapper(Guid eventId, DateTime occurredAt, Metadata meta)
    {
        EventId = eventId;
        OccurredAt = occurredAt;
        Meta = meta;
    }

    public Guid EventId { get; set; }
    public DateTime OccurredAt { get; set; }
    public Metadata Meta { get; set; }
}