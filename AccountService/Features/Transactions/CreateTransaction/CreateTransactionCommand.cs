using AccountService.Features.Transactions.Dto;
using MediatR;

namespace AccountService.Features.Transactions.CreateTransaction;

public class CreateTransactionCommand : IRequest<TransactionFullDto>
{
    public Guid AccountId { get; set; }
    public decimal Sum { get; set; }
    public TransactionType Type { get; set; }
    public string Description { get; set; } = string.Empty;
}