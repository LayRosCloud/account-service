namespace AccountService.Features.Transactions;

public interface ITransactionRepository
{
    // ReSharper disable once UnusedMember.Global
    Task<IList<Transaction>> FindAllAsync();
    // ReSharper disable once UnusedMember.Global
    Task<Transaction?> FindByIdAsync(Guid id);
    // ReSharper disable once UnusedMember.Global
    Task<IList<Transaction>> FindByAccountIdAsync(Guid accountId);
    Task<Transaction> CreateAsync(Transaction transaction);
    Task CreateRangeAsync(params Transaction[] transactions);
    // ReSharper disable once UnusedMember.Global
    Transaction Update(Transaction transaction);
    // ReSharper disable once UnusedMember.Global
    Transaction Delete(Transaction transaction);
}