using System.ComponentModel.DataAnnotations;
using AccountService.Features.Transactions.Dto;
using MediatR;

namespace AccountService.Features.Transactions.TransferBetweenAccounts;

public class TransferBetweenAccountsCommand : IRequest<TransactionFullDto>
{
    /// <summary>
    /// From Account id
    /// </summary>
    [Required] 
    public Guid AccountId { get; set; }

    /// <summary>
    /// To Account id
    /// </summary>
    [Required]
    public Guid CounterPartyAccountId { get; set; }

    /// <summary>
    /// Amount
    /// </summary>
    [Required]
    public decimal Sum { get; set; }

    /// <summary>
    /// Type transaction
    /// </summary>
    [Required]
    public TransactionType Type { get; set; }

    /// <summary>
    /// Description of transaction
    /// </summary>
    [Required]
    public string Description { get; set; } = string.Empty;
}