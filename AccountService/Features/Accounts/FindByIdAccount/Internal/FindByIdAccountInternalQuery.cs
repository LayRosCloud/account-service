using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace AccountService.Features.Accounts.FindByIdAccount.Internal;

public class FindByIdAccountInternalQuery : IRequest<Account>
{
    public FindByIdAccountInternalQuery(Guid accountId)
    {
        AccountId = accountId;
    }

    [SwaggerSchema("account id")] public Guid AccountId { get; }
}