using AccountService.Features.Accounts.Dto;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace AccountService.Features.Accounts.FindByIdAccount;

[SwaggerSchema(Description = "Data transfer object for find account by id",
    Required = new[] { "accountId" })]
public class FindByIdAccountQuery : IRequest<AccountResponseFullDto>
{
    public FindByIdAccountQuery(Guid accountId, long dateStart, long dateEnd)
    {
        AccountId = accountId;
        DateStart = dateStart;
        DateEnd = dateEnd;
    }

    [SwaggerSchema("date start transactions")] public long DateStart { get; }
    [SwaggerSchema("date end transactions")] public long DateEnd { get; }
    [SwaggerSchema("account id")] public Guid AccountId { get; }
}