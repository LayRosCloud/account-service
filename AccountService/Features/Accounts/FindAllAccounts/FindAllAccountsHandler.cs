using AccountService.Features.Accounts.Dto;
using AccountService.Utils.Data;
using AutoMapper;
using MediatR;

namespace AccountService.Features.Accounts.FindAllAccounts;

// ReSharper disable once UnusedMember.Global using Mediator
public class FindAllAccountsHandler : IRequestHandler<FindAllAccountsQuery, List<AccountResponseShortDto>>
{
    private readonly IAccountRepository _repository;
    private readonly ITransactionWrapper _wrapper;
    private readonly IMapper _mapper;

    public FindAllAccountsHandler(IMapper mapper, IAccountRepository repository, ITransactionWrapper wrapper)
    {
        _mapper = mapper;
        _repository = repository;
        _wrapper = wrapper;
    }

    public async Task<List<AccountResponseShortDto>> Handle(FindAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _wrapper.Execute(_ => FindAllAsync(), cancellationToken);
        return _mapper.Map<List<AccountResponseShortDto>>(accounts);
    }

    private async Task<IList<Account>> FindAllAsync()
    {
        var accounts = await _repository.FindAllAsync();
        return accounts;
    }
}