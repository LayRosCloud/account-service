using AccountService.Utils.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Broker.Events;

public class EventRepository : IEventRepository
{
    private readonly IStorageContext _storage;

    public EventRepository(IStorageContext storage)
    {
        _storage = storage;
    }

    public async Task<bool> ExistsEventByIdAsync(Guid id)
    {
        return await _storage.Events.AnyAsync(x => x.EventId == id);
    }

    public async Task<Event> CreateAsync(Event @event)
    {
        var result = await _storage.Events.AddAsync(@event);
        return result.Entity;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellation = new())
    {
        return _storage.SaveChangesAsync(cancellation);
    }
}