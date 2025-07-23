using MediatR;

namespace AccountService.Features.Accounts.DeleteAccount;

public class DeleteAccountCommand : IRequest<int>
{
    public Guid AccountId { get; }

    public DeleteAccountCommand(Guid accountId)
    {
        AccountId = accountId;
    }
}