using Swashbuckle.AspNetCore.Annotations;

namespace AccountService.Features.Transactions.Dto;

[SwaggerSchema("Transaction of account with balance")]
public class TransactionFullDto
{
    [SwaggerSchema("id")] public Guid Id { get; set; }

    [SwaggerSchema("account id")] public Guid AccountId { get; set; }

    [SwaggerSchema("counterparty account id")]
    public Guid? CounterPartyAccountId { get; set; }

    [SwaggerSchema("amount")] public decimal Sum { get; set; }

    [SwaggerSchema("currency of transaction")]
    public string Currency { get; set; } = string.Empty;

    [SwaggerSchema("type transaction Debit | Credit")]
    public TransactionType Type { get; set; }

    [SwaggerSchema("Description of transaction")]
    public string Description { get; set; } = string.Empty;

    [SwaggerSchema("Created of transaction in ticks")]
    public long CreatedAt { get; set; }
}