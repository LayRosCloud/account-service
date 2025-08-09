using AccountService.Features.Accounts;
using AccountService.Features.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AccountService.Utils.Data;

public interface IStorageContext
{
    public DbSet<Account> Accounts { get; }
    public DbSet<Transaction> Transactions { get; }
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}