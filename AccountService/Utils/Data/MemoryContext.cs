using AccountService.Features.Accounts;
using AccountService.Features.Transactions;

namespace AccountService.Utils.Data;

public sealed class MemoryContext : IDatabaseContext
{
    public List<Account> Accounts { get; } = new();
    public List<Transaction> Transactions { get; } = new();
}