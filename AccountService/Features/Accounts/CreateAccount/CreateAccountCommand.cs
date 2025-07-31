using System.ComponentModel.DataAnnotations;
using AccountService.Features.Accounts.Dto;
using MediatR;

namespace AccountService.Features.Accounts.CreateAccount;

public class CreateAccountCommand : IRequest<AccountResponseShortDto>
{
    /// <summary>
    /// Counterparty id
    /// </summary>]
    [Required]
    public Guid OwnerId { get; set; }

    /// <summary>
    /// account type: 1: Checking, 2: PerformOperation, 3: Credit
    /// </summary>
    [Required]
    public AccountType Type { get; set; }

    /// <summary>
    /// Code Currency
    /// </summary>
    [Required]
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// balance account
    /// </summary>
    [Required]
    public decimal Balance { get; set; }
    
    /// <summary>
    /// percent for credit account or invest
    /// </summary>
    public decimal? Percent { get; set; }
}