using AccountService.Features.Transactions.Dto;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace AccountService.Features.Transactions.CreateTransaction;

[SwaggerSchema(Description = "Data transfer object for create transaction",
    Required = new[] { "sum", "type", "description" })]
public class CreateTransactionCommand : IRequest<TransactionFullDto>
{
    [SwaggerSchema("account id")] public Guid AccountId { get; set; }

    [SwaggerSchema("amount transaction")] public decimal Sum { get; set; }

    [SwaggerSchema("type of transaction")] public TransactionType Type { get; set; }

    [SwaggerSchema("description of transaction")]
    public string Description { get; set; } = string.Empty;
}