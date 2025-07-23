using AccountService.Features.Accounts.Dto;
using MediatR;

namespace AccountService.Features.Accounts.UpdateAccount;

public class UpdateAccountCommand : IRequest<AccountResponseShortDto>
{
    public Guid Id { get; set; }
    public decimal? Percent { get; set; }
    public bool IsClosed { get; set; }
}