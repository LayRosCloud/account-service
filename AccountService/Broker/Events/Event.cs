namespace AccountService.Broker.Events;

public class Event
{
    public Event(Guid eventId, DateTime processedAt, string? handler)
    {
        EventId = eventId;
        ProcessedAt = processedAt;
        Handler = handler;
    }

    public Guid EventId { get; set; }
    public DateTime ProcessedAt { get; set; }
    public string? Handler { get; set; }
}