using AccountService.Features.Accounts.Dto;
using AccountService.Features.Accounts.FindByIdAccount.Internal;
using AutoMapper;
using MediatR;

namespace AccountService.Features.Accounts.FindByIdAccount;

// ReSharper disable once UnusedMember.Global using Mediator
public class FindByIdAccountHandler : IRequestHandler<FindByIdAccountQuery, AccountResponseFullDto>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public FindByIdAccountHandler(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<AccountResponseFullDto> Handle(FindByIdAccountQuery request, CancellationToken cancellationToken)
    {
        var query = new FindByIdAccountInternalQuery(request.AccountId);
        var account = await _mediator.Send(query, cancellationToken);
        return _mapper.Map<AccountResponseFullDto>(account);
    }
}