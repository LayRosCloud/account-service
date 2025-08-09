using AccountService.Features.Accounts.Dto;
using AccountService.Utils.Data;
using AccountService.Utils.Exceptions;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace AccountService.Features.Accounts.UpdatePercentAccount;

// ReSharper disable once UnusedMember.Global using Mediator
public class UpdateAccountPercentHandler : IRequestHandler<UpdateAccountPercentCommand, AccountResponseShortDto>
{
    private readonly IStorageContext _storage;
    private readonly IMapper _mapper;
    private readonly IAccountRepository _repository;
    private readonly ITransactionWrapper _wrapper;

    public UpdateAccountPercentHandler(IMapper mapper, IStorageContext storage, IAccountRepository repository, ITransactionWrapper wrapper)
    {
        _storage = storage;
        _repository = repository;
        _wrapper = wrapper;
        _mapper = mapper;
    }

    public async Task<AccountResponseShortDto> Handle(UpdateAccountPercentCommand request,
        CancellationToken cancellationToken)
    {
        var account = await _wrapper.Execute(_ => UpdateAsync(request, cancellationToken), cancellationToken);

        return _mapper.Map<AccountResponseShortDto>(account);
    }

    private async Task<Account> UpdateAsync(UpdateAccountPercentCommand request, CancellationToken cancellationToken)
    {
        var account = await ExistsAccount(request.Id);

        if (CheckOnCreditState(account.Type, request.Percent))
            throw new ValidationException("Percent is null of Credit type account");

        account.Percent = request.Percent;
        _repository.Update(account);
        await _storage.SaveChangesAsync(cancellationToken);
        return account;
    }

    private static bool CheckOnCreditState(AccountType type, decimal? percent)
    {
        return type == AccountType.Credit && percent == null;
    }

    private async Task<Account> ExistsAccount(Guid id)
    {
        var account = await _repository.FindByIdAsync(id);
        if (account == null)
            throw ExceptionUtils.GetNotFoundException("Account", id);

        return account;
    }
}