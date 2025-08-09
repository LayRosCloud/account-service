using AccountService.Features.Transactions;
using AccountService.Utils.Data;

namespace AccountService.Features.Accounts;

public class Account : ICloneable, IDateCreator
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public AccountType Type { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public decimal? Percent { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ClosedAt { get; set; }
    public byte[]? Version { get; set; }
    public IEnumerable<Transaction> Transactions => AccountTransactions.Concat(CounterPartyTransactions);
    public List<Transaction> AccountTransactions { get; } = new();
    public List<Transaction> CounterPartyTransactions { get; } = new();

    public object Clone()
    {
        return MemberwiseClone();
    }
}