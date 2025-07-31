using AccountService.Features.Accounts.Dto;
using AccountService.Utils.Data;
using AutoMapper;
using MediatR;

namespace AccountService.Features.Accounts.FindAllAccounts;

public class FindAllAccountsHandler : IRequestHandler<FindAllAccountsQuery, List<AccountResponseShortDto>>
{
    private readonly IDatabaseContext _database;
    private readonly IMapper _mapper;

    public FindAllAccountsHandler(IMapper mapper, IDatabaseContext database)
    {
        _mapper = mapper;
        _database = database;
    }

    public Task<List<AccountResponseShortDto>> Handle(FindAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = _database.Accounts;
        return Task.FromResult(_mapper.Map<List<AccountResponseShortDto>>(accounts));
    }
}