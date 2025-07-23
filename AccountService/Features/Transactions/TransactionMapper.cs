using AccountService.Features.Transactions.CreateTransaction;
using AccountService.Features.Transactions.Dto;
using AccountService.Features.Transactions.TransferBetweenAccounts;
using AutoMapper;

namespace AccountService.Features.Transactions;

public class TransactionMapper : Profile
{
    public TransactionMapper()
    {
        CreateMap<Transaction, TransactionFullDto>();
        CreateMap<CreateTransactionCommand, Transaction>();
        CreateMap<TransferBetweenAccountsCommand, Transaction>();
    }
}