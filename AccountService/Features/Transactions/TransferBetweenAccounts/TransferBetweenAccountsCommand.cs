using AccountService.Features.Transactions.Dto;
using MediatR;

namespace AccountService.Features.Transactions.TransferBetweenAccounts;

public class TransferBetweenAccountsCommand : IRequest<TransactionFullDto>
{
    public Guid AccountId { get; set; }
    public Guid CounterPartyAccountId { get; set; }
    public decimal Sum { get; set; }
    public string Description { get; set; } = string.Empty;
}