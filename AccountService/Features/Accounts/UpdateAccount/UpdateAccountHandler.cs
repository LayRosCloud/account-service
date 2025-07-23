using AccountService.Features.Accounts.Dto;
using AccountService.Utils.Data;
using AccountService.Utils.Exceptions;
using AccountService.Utils.Time;
using AutoMapper;
using MediatR;

namespace AccountService.Features.Accounts.UpdateAccount;

public class UpdateAccountHandler : IRequestHandler<UpdateAccountCommand, AccountResponseShortDto>
{
    private readonly DatabaseContext _databaseContext = DatabaseContext.Instance;
    private readonly IMapper _mapper;

    public UpdateAccountHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Task<AccountResponseShortDto> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = _databaseContext.Accounts.FirstOrDefault(acc => acc.Id == request.Id);
        
        if (account == null)
            throw new NotFoundException();

        account.ClosedAt = request.IsClosed ? TimeUtils.GetTicksFromCurrentDate() : null;

        account.Percent = request.Percent;

        return Task.FromResult(_mapper.Map<AccountResponseShortDto>(account));
    }

}
