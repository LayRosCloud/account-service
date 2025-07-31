namespace AccountService.Features.Transactions.Dto;

public class TransactionFullDto
{
    /// <summary>
    /// transaction id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// account id
    /// </summary>
    public Guid AccountId { get; set; }

    /// <summary>
    /// CounterParty account id
    /// </summary>
    public Guid? CounterPartyAccountId { get; set; }

    /// <summary>
    /// amount
    /// </summary>
    public decimal Sum { get; set; }

    /// <summary>
    /// currency of transaction
    /// </summary>
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// type transaction Debit | Credit
    /// </summary>
    public TransactionType Type { get; set; }

    /// <summary>
    /// Description of transaction
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Created of transaction in ticks
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }
}