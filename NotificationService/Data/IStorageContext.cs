using Microsoft.EntityFrameworkCore;
using NotificationService.Features.Broker;

namespace NotificationService.Data;

public interface IStorageContext
{
    public DbSet<EventEntity> Events { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}