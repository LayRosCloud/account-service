using MediatR;

namespace AccountService.Features.Accounts.FindByIdAccount.Internal;

public class FindByIdAccountInternalQuery : IRequest<Account>
{
    public FindByIdAccountInternalQuery(Guid accountId)
    {
        AccountId = accountId;
    }

    /// <summary>
    /// account id
    /// </summary>
    public Guid AccountId { get; }
}