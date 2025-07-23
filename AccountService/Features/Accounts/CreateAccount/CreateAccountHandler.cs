using AccountService.Features.Accounts.Dto;
using AccountService.Utils.Data;
using AccountService.Utils.Exceptions;
using AccountService.Utils.Time;
using AutoMapper;
using MediatR;

namespace AccountService.Features.Accounts.CreateAccount;

public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, AccountResponseShortDto>
{
    private readonly DatabaseContext _databaseContext = DatabaseContext.Instance;
    private readonly IMapper _mapper;

    public CreateAccountHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Task<AccountResponseShortDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        Guid? ownerId = _databaseContext.CounterParties.SingleOrDefault(id => request.OwnerId == id);

        if (ownerId == null)
        {
            throw new NotFoundException();
        }

        var account = _mapper.Map<Account>(request);
        account.Id = new Guid();
        account.CreatedAt = TimeUtils.GetTicksFromCurrentDate();
        _databaseContext.Accounts.Add(account);
        return Task.FromResult(_mapper.Map<AccountResponseShortDto>(account));
    }
}