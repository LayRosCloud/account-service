using AccountService.Features.Accounts.Dto;
using MediatR;

namespace AccountService.Features.Accounts.FindByIdAccount;

public class FindByIdAccountQuery : IRequest<AccountResponseFullDto>
{
    public FindByIdAccountQuery(Guid accountId)
    {
        AccountId = accountId;
    }

    /// <summary>
    /// account id
    /// </summary>
    public Guid AccountId { get; }
}