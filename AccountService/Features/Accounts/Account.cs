using AccountService.Features.Transactions;

namespace AccountService.Features.Accounts;

public class Account : ICloneable
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public AccountType Type { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public decimal? Percent { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ClosedAt { get; set; }
    public List<Transaction> Transactions { get; } = new();

    public object Clone()
    {
        return MemberwiseClone();
    }
}