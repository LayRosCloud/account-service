using AccountService.Features.Accounts.Dto;
using MediatR;

namespace AccountService.Features.Accounts.FIndByAccount;

public class FindByIdAccountQuery : IRequest<AccountResponseFullDto>
{
    public FindByIdAccountQuery(Guid accountId)
    {
        AccountId = accountId;
    }

    public Guid AccountId { get; }
}