using AccountService.Features.Accounts;
using AccountService.Features.Transactions;

namespace AccountService.Utils.Data;

public interface IDatabaseContext
{
    public List<Account> Accounts { get; }
    public List<Transaction> Transactions { get; }
    public List<Guid> CounterParties { get; }
}