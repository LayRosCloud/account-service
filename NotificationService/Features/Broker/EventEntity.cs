namespace NotificationService.Features.Broker;

public class EventEntity
{
    public EventEntity(Guid eventId, DateTime processedAt, string handler)
    {
        EventId = eventId;
        ProcessedAt = processedAt;
        Handler = handler;
    }

    public Guid EventId { get; set; }
    public DateTime ProcessedAt { get; set; }
    public string Handler { get; set; }
}