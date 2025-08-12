using AccountService.Utils.Exceptions;
using MediatR;

namespace AccountService.Features.Accounts.FindByIdAccount.Internal;

// ReSharper disable once UnusedMember.Global using Mediator
public class FindByIdAccountInternalHandler : IRequestHandler<FindByIdAccountInternalQuery, Account>
{
    private readonly IAccountRepository _repository;

    public FindByIdAccountInternalHandler(IAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task<Account> Handle(FindByIdAccountInternalQuery request, CancellationToken cancellationToken)
    {
        var account = await FindByIdAsync(request.AccountId);
        return account;
    }

    private async Task<Account> FindByIdAsync(Guid accountId)
    {
        var account = await _repository.FindByIdAsync(accountId);
        if (account == null)
            throw ExceptionUtils.GetNotFoundException("Account", accountId);
        return account;
    }
}