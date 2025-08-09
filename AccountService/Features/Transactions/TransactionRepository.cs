using AccountService.Utils.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Features.Transactions;

public class TransactionRepository : ITransactionRepository
{
    private readonly IStorageContext _storage;

    public TransactionRepository(IStorageContext storage)
    {
        _storage = storage;
    }

    public async Task<IList<Transaction>> FindAllAsync()
    {
         var list = await _storage.Transactions.ToListAsync();
         return list;
    }

    public async Task<Transaction?> FindByIdAsync(Guid id)
    {
        var item = await _storage.Transactions.FirstOrDefaultAsync(x => x.Id == id);
        return item;
    }

    public async Task<IList<Transaction>> FindByAccountIdAsync(Guid accountId)
    {
        var items = await _storage.Transactions.Where(x => x.AccountId == accountId).ToListAsync();
        return items;
    }

    public async Task<Transaction> CreateAsync(Transaction transaction)
    {
        var item = await _storage.Transactions.AddAsync(transaction);
        return item.Entity;
    }

    public async Task CreateRangeAsync(params Transaction[] transactions)
    {
        await _storage.Transactions.AddRangeAsync(transactions);
    }

    public Transaction Update(Transaction transaction)
    {
        var item =  _storage.Transactions.Update(transaction);
        return item.Entity;
    }

    public Transaction Delete(Transaction transaction)
    {
        var item = _storage.Transactions.Remove(transaction);
        return item.Entity;
    }
}