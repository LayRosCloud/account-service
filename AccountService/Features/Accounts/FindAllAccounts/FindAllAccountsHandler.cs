using AccountService.Features.Accounts.Dto;
using AccountService.Utils.Data;
using AutoMapper;
using MediatR;

namespace AccountService.Features.Accounts.FindAllAccounts;

public class FindAllAccountsHandler : IRequestHandler<FindAllAccountsQuery, List<AccountResponseShortDto>>
{
    private readonly DatabaseContext _databaseContext = DatabaseContext.Instance;
    private readonly IMapper _mapper;

    public FindAllAccountsHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Task<List<AccountResponseShortDto>> Handle(FindAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = _databaseContext.Accounts;
        return Task.FromResult(_mapper.Map<List<AccountResponseShortDto>>(accounts));
    }
}