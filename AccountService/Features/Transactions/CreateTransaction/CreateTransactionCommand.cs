using System.ComponentModel.DataAnnotations;
using AccountService.Features.Transactions.Dto;
using MediatR;

namespace AccountService.Features.Transactions.CreateTransaction;

public class CreateTransactionCommand : IRequest<TransactionFullDto>
{
    /// <summary>
    /// account id
    /// </summary>
    [Required] 
    public Guid AccountId { get; set; }

    /// <summary>
    /// amount transaction
    /// </summary>
    [Required] 
    public decimal Sum { get; set; }

    /// <summary>
    /// type of transaction
    /// </summary>
    [Required] 
    public TransactionType Type { get; set; }

    /// <summary>
    /// description of transaction
    /// </summary>
    [Required]
    public string Description { get; set; } = string.Empty;
}