using AccountService.Features.Accounts.Dto;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace AccountService.Features.Accounts.FindByIdAccount;

public class FindByIdAccountQuery : IRequest<AccountResponseFullDto>
{
    public FindByIdAccountQuery(Guid accountId)
    {
        AccountId = accountId;
    }

    [SwaggerSchema("account id")] public Guid AccountId { get; }
}