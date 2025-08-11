using System.ComponentModel.DataAnnotations;
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
    /// <summary>
    /// Account id
    /// </summary>
    [Required]
    public Guid AccountId { get; }
}