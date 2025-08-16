using AccountService.Broker;
using AccountService.Features.Accounts.Dto;
using AccountService.Features.Users.VerifyUser;
using AccountService.Utils.Broker;
using AccountService.Utils.Data;
using AccountService.Utils.Exceptions;
using AutoMapper;
using Broker.AccountService;
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
    private readonly IProducer<AccountOpenedEvent> _producer;
    private readonly HttpContext _context;

    public CreateAccountHandler(IMapper mapper, IMediator mediator, IStorageContext storage, IAccountRepository repository, ITransactionWrapper wrapper, HttpContext context, IProducer<AccountOpenedEvent> producer)
    {
        _mapper = mapper;
        _mediator = mediator;
        _storage = storage;
        _repository = repository;
        _wrapper = wrapper;
        _context = context;
        _producer = producer;
    }

    public async Task<AccountResponseShortDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var result = await _wrapper.Execute(_ => CreateAsync(request, cancellationToken), cancellationToken);
        await ProduceAccountAsync(result);
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
        await ProduceAccountAsync(result);
        return result;
    }

    private async Task<bool> ExistUserAsync(Guid id, CancellationToken cancellationToken)
    {
        var verifyUser = new VerifyUserCommand(id);
        var hasOwnerUser = await _mediator.Send(verifyUser, cancellationToken);
        return hasOwnerUser;
    }

    private async Task ProduceAccountAsync(Account account)
    {
        var correlation = (string)_context.Items["X-Correlation-ID"]!;
        var meta = MetaCreator.Create(Guid.Parse(correlation), 
            MetaCreator.AccountCreate);
        var accountEvent = new AccountOpenedEvent(Guid.NewGuid(), DateTime.UtcNow, meta, account.Currency)
            {
                Type = account.Type,
                AccountId = account.Id,
                OwnerId = account.OwnerId
            };
        await _producer.ProduceAsync(accountEvent);
    }
}