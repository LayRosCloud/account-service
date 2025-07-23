using AccountService.Features.Accounts.CreateAccount;
using AccountService.Features.Accounts.Dto;
using AutoMapper;

namespace AccountService.Features.Accounts;

public class AccountMapper : Profile
{
    public AccountMapper()
    {
        CreateMap<CreateAccountCommand, Account>();
        CreateMap<Account, AccountResponseShortDto>();
        CreateMap<Account, AccountResponseFullDto>();
    }
}