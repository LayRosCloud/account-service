using MediatR;

namespace AccountService.Features.Accounts.FindAllAccounts;

public class FindAllAccountsQuery : IRequest<ICollection<Account>>
{

}