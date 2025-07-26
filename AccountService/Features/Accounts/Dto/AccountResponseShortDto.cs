using Swashbuckle.AspNetCore.Annotations;

namespace AccountService.Features.Accounts.Dto;

[SwaggerSchema("Account response without transactions")]
public class AccountResponseShortDto
{
    [SwaggerSchema("Account id")]
    public Guid Id { get; set; }

    [SwaggerSchema("Owner id")]
    public Guid OwnerId { get; set; }

    [SwaggerSchema("Type of account checking, credit, debit")]
    public AccountType Type { get; set; }

    [SwaggerSchema("Currency of account RUB, EUR and others")]
    public string Currency { get; set; } = string.Empty;

    [SwaggerSchema("Balance of account")]
    public decimal Balance { get; set; }

    [SwaggerSchema("Percent of account")]
    public decimal? Percent { get; set; }

    [SwaggerSchema("Created account in ticks")]
    public long CreatedAt { get; set; }

    [SwaggerSchema("Closed account in ticks")]
    public long? ClosedAt { get; set; }
}