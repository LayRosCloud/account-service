using AccountService.Features.Accounts.Dto;
using MediatR;

namespace AccountService.Features.Accounts.CreateAccount;

public class CreateAccountCommand : IRequest<AccountResponseShortDto>
{
    public Guid OwnerId { get; set; }
    public AccountType Type { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public decimal? Percent { get; set; }
}