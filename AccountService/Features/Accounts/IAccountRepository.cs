namespace AccountService.Features.Accounts;

public interface IAccountRepository
{
    Task<IList<Account>> FindAllAsync();
    Task<bool> HasAccountAndOwnerAsync(Guid accountId, Guid ownerId);
    Task<Account?> FindByIdAsync(Guid id, bool isIncludeTransactions = false);
    Task<Account> CreateAsync(Account account);
    Account Update(Account account);
    Account Delete(Account account);
}