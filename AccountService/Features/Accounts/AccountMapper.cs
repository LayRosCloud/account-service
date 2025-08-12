using AccountService.Features.Accounts.CreateAccount;
using AccountService.Features.Accounts.Dto;
using AccountService.Features.Transactions.Dto;
using AutoMapper;

namespace AccountService.Features.Accounts;

public class AccountMapper : Profile
{
    public AccountMapper()
    {
        CreateMap<CreateAccountCommand, Account>();
        CreateMap<Account, AccountResponseShortDto>();
        CreateMap<Account, AccountResponseFullDto>()
            .AfterMap((src, dest, context) =>
            {
                dest.Transactions = src.AccountTransactions
                    .Select(x => context.Mapper.Map<TransactionFullDto>(x))
                    .Concat(src.CounterPartyTransactions
                        .Select(t => context.Mapper.Map<TransactionFullDto>(t))).ToList();
            });
    }
}