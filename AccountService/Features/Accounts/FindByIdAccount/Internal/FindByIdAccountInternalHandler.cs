using AccountService.Utils.Data;
using AccountService.Utils.Exceptions;
using MediatR;

namespace AccountService.Features.Accounts.FindByIdAccount.Internal;

public class FindByIdAccountInternalHandler : IRequestHandler<FindByIdAccountInternalQuery, Account>
{
    private readonly DatabaseContext _databaseContext = DatabaseContext.Instance;

    public Task<Account> Handle(FindByIdAccountInternalQuery request, CancellationToken cancellationToken)
    {
        var account = _databaseContext.Accounts.SingleOrDefault(acc => acc.Id == request.AccountId);
        if (account == null)
            throw ExceptionUtils.GetNotFoundException("Account", request.AccountId);
        return Task.FromResult(account);
    }
}