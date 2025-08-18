using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NotificationService.Data.Configuration;
using NotificationService.Features.Broker;

namespace NotificationService.Data;

public class DatabaseContext : DbContext, IStorageContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<EventEntity> Events { get; set; } = null!;
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await Database.BeginTransactionAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EventConfiguration());
    }
}