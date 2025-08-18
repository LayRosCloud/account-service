namespace NotificationService.Features.Broker;

public interface IEventRepository
{
    Task<EventEntity> CreateAsync(EventEntity @event);
    Task<bool> ExistsEventAsync(Guid eventId);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}