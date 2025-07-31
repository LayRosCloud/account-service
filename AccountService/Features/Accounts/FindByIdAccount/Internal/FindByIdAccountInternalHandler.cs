using AccountService.Utils.Data;
using AccountService.Utils.Exceptions;
using MediatR;

namespace AccountService.Features.Accounts.FindByIdAccount.Internal;

// ReSharper disable once UnusedMember.Global using Mediator
public class FindByIdAccountInternalHandler : IRequestHandler<FindByIdAccountInternalQuery, Account>
{
    private readonly IDatabaseContext _database;

    public FindByIdAccountInternalHandler(IDatabaseContext database)
    {
        _database = database;
    }

    public Task<Account> Handle(FindByIdAccountInternalQuery request, CancellationToken cancellationToken)
    {
        var account = _database.Accounts.SingleOrDefault(acc => acc.Id == request.AccountId);
        if (account == null)
            throw ExceptionUtils.GetNotFoundException("Account", request.AccountId);
        return Task.FromResult(account);
    }
}