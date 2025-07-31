using AccountService.Features.Accounts.Dto;
using AccountService.Features.Users.VerifyUser;
using AccountService.Utils.Data;
using AccountService.Utils.Exceptions;
using AccountService.Utils.Time;
using AutoMapper;
using MediatR;

namespace AccountService.Features.Accounts.CreateAccount;

public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, AccountResponseShortDto>
{
    private readonly IDatabaseContext _database;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateAccountHandler(IMapper mapper, IMediator mediator, IDatabaseContext database)
    {
        _mapper = mapper;
        _mediator = mediator;
        _database = database;
    }

    public Task<AccountResponseShortDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var verifyUser = new VerifyUserCommand(request.OwnerId);
        var hasOwnerUser = _mediator.Send(verifyUser, cancellationToken).Result;

        if (hasOwnerUser == false) throw ExceptionUtils.GetNotFoundException("Owner", request.OwnerId);

        var account = _mapper.Map<Account>(request);
        account.Id = Guid.NewGuid();
        account.CreatedAt = TimeUtils.GetTicksFromCurrentDate();
        _database.Accounts.Add(account);
        return Task.FromResult(_mapper.Map<AccountResponseShortDto>(account));
    }
}