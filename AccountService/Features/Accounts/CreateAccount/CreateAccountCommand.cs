using AccountService.Features.Accounts.Dto;
using AccountService.Utils;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace AccountService.Features.Accounts.CreateAccount;

[SwaggerSchema(Description = "Data transfer object for create account", 
    Required = new []{ "ownerId", "type", "currency", "balance" })]
public class CreateAccountCommand : IRequest<AccountResponseShortDto>
{
    [SwaggerSchema("counterparty id")]
    public Guid OwnerId { get; set; }

    [SwaggerSchema("account type: 1: Checking, 2: Deposit, 3: Credit")]
    public AccountType Type { get; set; }

    [SwaggerSchema("currency code")]
    public string Currency { get; set; } = string.Empty;

    [SwaggerSchema("balance account")]
    public decimal Balance { get; set; }

    [SwaggerSchema("percent for credit account or invest")]
    public decimal? Percent { get; set; }
}