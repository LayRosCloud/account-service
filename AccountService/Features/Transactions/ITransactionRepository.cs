namespace AccountService.Features.Transactions;

public interface ITransactionRepository
{
    Task<IList<Transaction>> FindAllAsync();
    Task<Transaction?> FindByIdAsync(Guid id);
    Task<IList<Transaction>> FindByAccountIdAsync(Guid accountId);
    Task<Transaction> CreateAsync(Transaction transaction);
    Task CreateRangeAsync(params Transaction[] transactions);
    Transaction Update(Transaction transaction);
    Transaction Delete(Transaction transaction);
}