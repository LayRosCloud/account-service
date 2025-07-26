using AccountService.Features.Accounts.Dto;
using AccountService.Features.Accounts.FindByIdAccount.Internal;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace AccountService.Features.Accounts.UpdateTypeAccount;

public class UpdateTypeAccountHandler : IRequestHandler<UpdateTypeAccountCommand, AccountResponseShortDto>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public UpdateTypeAccountHandler(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public Task<AccountResponseShortDto> Handle(UpdateTypeAccountCommand request, CancellationToken cancellationToken)
    {
        var query = new FindByIdAccountInternalQuery(request.AccountId);
        var account = _mediator.Send(query, cancellationToken).Result;
        if (account.Type != AccountType.Checking && request.Type != AccountType.Checking)
            throw new ValidationException("You cannot change not Checking type");

        if (request.Type == AccountType.Deposit && account.Balance < 0)
            throw new ValidationException("You don't change to Deposit type with negative balance");
        if (request.Type == AccountType.Credit && account.Percent == null)
            throw new ValidationException("You don't change to Credit type without percent");
        account.Type = request.Type;
        return Task.FromResult(_mapper.Map<AccountResponseShortDto>(account));
    }
}