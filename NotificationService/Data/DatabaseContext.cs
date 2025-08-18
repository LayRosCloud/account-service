using Microsoft.EntityFrameworkCore;
using NotificationService.Data.Configuration;
using NotificationService.Features.Broker;

namespace NotificationService.Data;

public class DatabaseContext : DbContext, IStorageContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<EventEntity> Events { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EventConfiguration());
    }
}