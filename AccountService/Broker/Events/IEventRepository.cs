namespace AccountService.Broker.Events;

public interface IEventRepository
{
    Task<bool> ExistsEventByIdAsync(Guid id);
    Task<Event> CreateAsync(Event @event);
    Task<int> SaveChangesAsync(CancellationToken cancellation = new());
}