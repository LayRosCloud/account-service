using System.ComponentModel;
using AccountService.Features.Accounts.Dto;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace AccountService.Features.Accounts.UpdatePercentAccount;

public class UpdateAccountPercentCommand : IRequest<AccountResponseShortDto>
{
    /// <summary>
    /// account id
    /// </summary>
    [Required, ReadOnly(true)]
    public Guid Id { get; set; }
    /// <summary>
    /// percent for credit account
    /// </summary>
    public decimal? Percent { get; set; }
}