using AccountService.Utils.Data;
using MediatR;

namespace AccountService.Features.Accounts.HasAccountWithCounterParty;

// ReSharper disable once UnusedMember.Global using Mediator
public class HasAccountWithCounterPartyHandler : IRequestHandler<HasAccountWithCounterPartyCommand, bool>
{
    private readonly IAccountRepository _repository;
    private readonly ITransactionWrapper _wrapper;

    public HasAccountWithCounterPartyHandler(IAccountRepository repository, ITransactionWrapper wrapper)
    {
        _repository = repository;
        _wrapper = wrapper;
    }

    public async Task<bool> Handle(HasAccountWithCounterPartyCommand request, CancellationToken cancellationToken)
    {
        var hasAccount = await _wrapper.Execute(_ => HasAccountAndOwnerAsync(request), cancellationToken);
        return hasAccount;
    }

    private async Task<bool> HasAccountAndOwnerAsync(HasAccountWithCounterPartyCommand request)
    {
        var hasAccount = await _repository.HasAccountAndOwnerAsync(request.AccountId, request.OwnerId);
        return hasAccount;
    }
}