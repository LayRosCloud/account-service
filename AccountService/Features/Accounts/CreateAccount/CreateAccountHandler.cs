using AccountService.Features.Accounts.Dto;
using AccountService.Features.Users.VerifyUser;
using AccountService.Utils.Data;
using AccountService.Utils.Exceptions;
using AutoMapper;
using MediatR;

namespace AccountService.Features.Accounts.CreateAccount;

// ReSharper disable once UnusedMember.Global using Mediator
public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, AccountResponseShortDto>
{
    private readonly IStorageContext _storage;
    private readonly ITransactionWrapper _wrapper;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IAccountRepository _repository;

    public CreateAccountHandler(IMapper mapper, IMediator mediator, IStorageContext storage, IAccountRepository repository, ITransactionWrapper wrapper)
    {
        _mapper = mapper;
        _mediator = mediator;
        _storage = storage;
        _repository = repository;
        _wrapper = wrapper;
    }

    public async Task<AccountResponseShortDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var result = await _wrapper.Execute(_ => CreateAsync(request, cancellationToken), cancellationToken);
        return _mapper.Map<AccountResponseShortDto>(result);
    }

    private async Task<Account> CreateAsync(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        if (await ExistUserAsync(request.OwnerId, cancellationToken) == false)
            throw ExceptionUtils.GetNotFoundException("OwnerId", request.OwnerId);

        var account = _mapper.Map<Account>(request);
        account.Id = Guid.NewGuid();
        var result = await _repository.CreateAsync(account);
        await _storage.SaveChangesAsync(cancellationToken);
        return result;
    }

    private async Task<bool> ExistUserAsync(Guid id, CancellationToken cancellationToken)
    {
        var verifyUser = new VerifyUserCommand(id);
        var hasOwnerUser = await _mediator.Send(verifyUser, cancellationToken);
        return hasOwnerUser;
    }
}