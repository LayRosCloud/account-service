using AccountService.Utils.Data;
using MediatR;

namespace AccountService.Features.Accounts.HasAccountWithCounterParty;

public class HasAccountWithCounterPartyHandler : IRequestHandler<HasAccountWithCounterPartyCommand, bool>
{
    private readonly DatabaseContext _databaseContext = DatabaseContext.Instance;

    public Task<bool> Handle(HasAccountWithCounterPartyCommand request, CancellationToken cancellationToken)
    {
        var account = _databaseContext.Accounts.SingleOrDefault(acc =>
            acc.Id == request.AccountId && acc.OwnerId == request.OwnerId);
        return Task.FromResult(account != null);
    }
}