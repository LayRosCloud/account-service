using AccountService.Features.Accounts;
using AccountService.Utils.Data;

namespace AccountService.Features.Transactions;

public class Transaction: IDateCreator
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid? CounterPartyAccountId { get; set; }
    public decimal Sum { get; set; }
    public string Currency { get; set; } = string.Empty;
    public TransactionType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public virtual Account? Account { get; set; }
    public virtual Account? CounterPartyAccount { get; set; }
}