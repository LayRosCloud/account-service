using AccountService.Features.Accounts.Dto;
using AccountService.Utils.Data;
using AccountService.Utils.Exceptions;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace AccountService.Features.Accounts.UpdatePercentAccount;

public class UpdateAccountPercentHandler : IRequestHandler<UpdateAccountPercentCommand, AccountResponseShortDto>
{
    private readonly IDatabaseContext _database;
    private readonly IMapper _mapper;

    public UpdateAccountPercentHandler(IMapper mapper, IDatabaseContext database)
    {
        _database = database;
        _mapper = mapper;
    }

    public Task<AccountResponseShortDto> Handle(UpdateAccountPercentCommand request,
        CancellationToken cancellationToken)
    {
        var account = _database.Accounts.FirstOrDefault(acc => acc.Id == request.Id);

        if (account == null)
            throw ExceptionUtils.GetNotFoundException("Account", request.Id);
        if (account.Type == AccountType.Credit && request.Percent == null)
            throw new ValidationException("Percent is null of Credit type account");
        account.Percent = request.Percent;

        return Task.FromResult(_mapper.Map<AccountResponseShortDto>(account));
    }
}