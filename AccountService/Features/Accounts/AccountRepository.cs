using AccountService.Utils.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Features.Accounts;

public class AccountRepository : IAccountRepository
{
    private readonly IStorageContext _storage;

    public AccountRepository(IStorageContext storage)
    {
        _storage = storage;
    }

    public async Task<IList<Account>> FindAllAsync()
    {
        return await _storage.Accounts.ToListAsync();
    }

    public async Task<bool> HasAccountAndOwnerAsync(Guid accountId, Guid ownerId)
    {
        return await _storage.Accounts.AnyAsync(x => x.Id == accountId && x.OwnerId == ownerId);
    }

    public async Task<Account?> FindByIdAsync(Guid id, bool isIncludeTransactions = false)
    {
        var result = _storage.Accounts.AsQueryable();
        if (isIncludeTransactions)
        {
            result = result.Include(x => x.AccountTransactions)
                .Include(x => x.CounterPartyTransactions);
        }

        return await result.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Account> CreateAsync(Account account)
    {
        var result = await _storage.Accounts.AddAsync(account);
        return result.Entity;
    }

    public Account Update(Account account)
    {
        var result = _storage.Accounts.Update(account);
        return result.Entity;
    }

    public async Task<IList<Account>> FindAllByPercentNotNullAndNotClosedAtAsync()
    {
        var result = await _storage.Accounts.Where(x => x.Percent.HasValue).ToListAsync();
        return result;
    }
}