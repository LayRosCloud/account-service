using AccountService.Features.Transactions.Dto;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace AccountService.Features.Transactions.FindByAccountIdTransactions;

[SwaggerSchema(Description = "Data transfer object for find query transaction",
    Required = new[] { "accountId" })]
public class FindByAccountIdTransactionsQuery : IRequest<List<TransactionFullDto>>
{
    public FindByAccountIdTransactionsQuery(Guid accountId)
    {
        AccountId = accountId;
    }

    public Guid AccountId { get; }
}