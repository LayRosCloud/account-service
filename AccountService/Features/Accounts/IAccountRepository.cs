namespace AccountService.Features.Accounts;

public interface IAccountRepository
{
    Task<IList<Account>> FindAllAsync();
    Task<IList<Account>> FindAllByOwnerIdAsync(Guid ownerId);
    Task<bool> HasAccountAndOwnerAsync(Guid accountId, Guid ownerId);
    Task<Account?> FindByIdAsync(Guid id, bool isIncludeTransactions = false);
    Task<Account> CreateAsync(Account account);
    Account Update(Account account);
    void UpdateRange(IEnumerable<Account> account);
    Task<IList<Account>> FindAllByPercentNotNullAndNotClosedAtAsync();
}