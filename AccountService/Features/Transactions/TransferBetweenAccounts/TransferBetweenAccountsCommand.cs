using AccountService.Features.Transactions.Dto;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace AccountService.Features.Transactions.TransferBetweenAccounts;

[SwaggerSchema(Description = "Data transfer object for create transfer transaction",
    Required = new[] { "sum", "description" })]
public class TransferBetweenAccountsCommand : IRequest<TransactionFullDto>
{
    [SwaggerSchema("from account id")] public Guid AccountId { get; set; }

    [SwaggerSchema("to account id")] public Guid CounterPartyAccountId { get; set; }

    [SwaggerSchema("amount of transaction")]
    public decimal Sum { get; set; }

    [SwaggerSchema("description transaction")]
    public string Description { get; set; } = string.Empty;
}