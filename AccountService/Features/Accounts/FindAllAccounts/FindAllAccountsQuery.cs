using AccountService.Features.Accounts.Dto;
using MediatR;

namespace AccountService.Features.Accounts.FindAllAccounts;

public class FindAllAccountsQuery : IRequest<List<AccountResponseShortDto>>
{

}