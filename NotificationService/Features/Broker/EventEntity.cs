namespace NotificationService.Features.Broker;

public class EventEntity
{
    public Guid EventId { get; set; }
    public DateTime ProcessedAt { get; set; }
    public string Handler { get; set; } = string.Empty;
}