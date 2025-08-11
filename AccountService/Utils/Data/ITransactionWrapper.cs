using Microsoft.EntityFrameworkCore.Storage;

namespace AccountService.Utils.Data;

public interface ITransactionWrapper
{
    Task<TEntity> Execute<TEntity>(Func<IDbContextTransaction?, Task<TEntity>> function, CancellationToken cancellationToken);
}