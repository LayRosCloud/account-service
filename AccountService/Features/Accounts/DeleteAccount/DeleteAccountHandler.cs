using AccountService.Utils.Data;
using AccountService.Utils.Exceptions;
using AccountService.Utils.Time;
using MediatR;

namespace AccountService.Features.Accounts.DeleteAccount;

public class DeleteAccountHandler : IRequestHandler<DeleteAccountCommand, int>
{
    private readonly DatabaseContext _databaseContext = DatabaseContext.Instance;

    public Task<int> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = _databaseContext.Accounts.FirstOrDefault(x => x.Id == request.AccountId);
        
        if (account == null)
            throw new NotFoundException();

        account.ClosedAt = TimeUtils.GetTicksFromCurrentDate();
        return Task.FromResult(1);
    }
}