namespace AccountService.Features.Accounts.Transactions;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid? CounterPartyId { get; set; }
    public decimal Sum { get; set; }
    public string Currency { get; set; } = string.Empty;
    public TransactionType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public long CreatedAt { get; set; }

}