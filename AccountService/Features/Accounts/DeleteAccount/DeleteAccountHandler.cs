using AccountService.Utils.Data;
using AccountService.Utils.Exceptions;
using AccountService.Utils.Time;
using MediatR;

namespace AccountService.Features.Accounts.DeleteAccount;

public class DeleteAccountHandler : IRequestHandler<DeleteAccountCommand, Unit>
{
    private readonly IDatabaseContext _database;

    public DeleteAccountHandler(IDatabaseContext database)
    {
        _database = database;
    }

    public Task<Unit> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = _database.Accounts.FirstOrDefault(x => x.Id == request.AccountId);

        if (account == null)
            throw ExceptionUtils.GetNotFoundException("Account", request.AccountId);

        account.ClosedAt ??= TimeUtils.GetTicksFromCurrentDate();

        return Task.FromResult(Unit.Value);
    }
}