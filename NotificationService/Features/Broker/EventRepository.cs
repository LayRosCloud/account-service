using Microsoft.EntityFrameworkCore;
using NotificationService.Data;

namespace NotificationService.Features.Broker;

public class EventRepository : IEventRepository
{
    private readonly IStorageContext _storage;

    public EventRepository(IStorageContext storage)
    {
        _storage = storage;
    }

    public async Task<EventEntity> CreateAsync(EventEntity @event)
    {
        var entry = await _storage.Events.AddAsync(@event);
        return entry.Entity;
    }

    public async Task<bool> ExistsEventAsync(Guid eventId)
    {
        return await _storage.Events.AnyAsync(x => x.EventId == eventId);
    }
}