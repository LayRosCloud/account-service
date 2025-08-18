using AccountService.Features.Accounts.Dto;
using AccountService.Features.Accounts.FindByIdAccount.Internal;
using AccountService.Utils.Data;
using AutoMapper;
using Broker.AccountService;
using FluentValidation;
using MediatR;

namespace AccountService.Features.Accounts.UpdateTypeAccount;

// ReSharper disable once UnusedMember.Global using Mediator
public class UpdateAccountTypeHandler : IRequestHandler<UpdateAccountTypeCommand, AccountResponseShortDto>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ITransactionWrapper _wrapper;
    private readonly IAccountRepository _repository;
    private readonly IStorageContext _storage;

    public UpdateAccountTypeHandler(IMapper mapper, IMediator mediator, IAccountRepository repository, IStorageContext storage, ITransactionWrapper wrapper)
    {
        _mapper = mapper;
        _mediator = mediator;
        _repository = repository;
        _storage = storage;
        _wrapper = wrapper;
    }

    public async Task<AccountResponseShortDto> Handle(UpdateAccountTypeCommand request, CancellationToken cancellationToken)
    {
        var result = await _wrapper.Execute(_ => UpdateTypeAccountAsync(request, cancellationToken), cancellationToken);
        return _mapper.Map<AccountResponseShortDto>(result);
    }

    private async Task<Account> UpdateTypeAccountAsync(UpdateAccountTypeCommand request, CancellationToken cancellationToken)
    {
        var query = new FindByIdAccountInternalQuery(request.AccountId);
        var account = await _mediator.Send(query, cancellationToken);
        if (account.Type != AccountType.Checking && request.Type != AccountType.Checking)
            throw new ValidationException("You cannot change not Checking type");

        // ReSharper disable once ConvertIfStatementToSwitchStatement the solution becomes less readable.
        if (request.Type == AccountType.Deposit && account.Balance < 0)
            throw new ValidationException("You don't change to Deposit type with negative balance");
        if (request.Type == AccountType.Credit && account.Percent == null)
            throw new ValidationException("You don't change to Credit type without percent");


        account.Type = request.Type;
        var result = _repository.Update(account);
        await _storage.SaveChangesAsync(cancellationToken);
        return result;
    }
}