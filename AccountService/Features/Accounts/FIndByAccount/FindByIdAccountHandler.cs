using AccountService.Features.Accounts.Dto;
using AccountService.Utils.Data;
using AutoMapper;
using MediatR;

namespace AccountService.Features.Accounts.FIndByAccount;

public class FindByIdAccountHandler : IRequestHandler<FindByIdAccountQuery, AccountResponseFullDto>
{
    private readonly DatabaseContext _databaseContext = DatabaseContext.Instance;
    private readonly IMapper _mapper;

    public FindByIdAccountHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Task<AccountResponseFullDto> Handle(FindByIdAccountQuery request, CancellationToken cancellationToken)
    {
        var account = _databaseContext.Accounts.FirstOrDefault(acc => acc.Id == request.AccountId);
        return Task.FromResult(_mapper.Map<AccountResponseFullDto>(account));
    }
}