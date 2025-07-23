using MediatR;

namespace AccountService.Features.Accounts.FindAllAccounts;

public class FindAllAccountsHandler : IRequestHandler<FindAllAccountsQuery, ICollection<Account>>
{
    public Task<ICollection<Account>> Handle(FindAllAccountsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}