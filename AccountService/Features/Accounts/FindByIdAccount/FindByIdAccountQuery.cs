using AccountService.Features.Accounts.Dto;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace AccountService.Features.Accounts.FindByIdAccount;

[SwaggerSchema(Description = "Data transfer object for find account by id",
    Required = new[] { "accountId" })]
public class FindByIdAccountQuery : IRequest<AccountResponseFullDto>
{
    [SwaggerSchema("account id")]
    public Guid AccountId { get; }

    public FindByIdAccountQuery(Guid accountId)
    {
        AccountId = accountId;
    }
}