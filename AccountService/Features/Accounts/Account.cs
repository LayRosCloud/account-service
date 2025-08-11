using AccountService.Features.Transactions;
using AccountService.Utils.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public long Version { get; set; }
    public IList<Transaction> Transactions => AccountTransactions.Concat(CounterPartyTransactions).ToList();
    public List<Transaction> AccountTransactions { get; } = new();
    public List<Transaction> CounterPartyTransactions { get; } = new();

    public object Clone()
    {
        return MemberwiseClone();
    }
}