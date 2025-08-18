using AccountService.Broker;
using AccountService.Features.Accounts;
using AccountService.Features.Accounts.FindByIdAccount.Internal;
using AccountService.Features.Transactions.Dto;
using AccountService.Features.Transactions.Utils.Transfer;
using AccountService.Utils.Broker;
using AccountService.Utils.Data;
using AutoMapper;
using Broker.AccountService;
using MediatR;

namespace AccountService.Features.Transactions.TransferBetweenAccounts;

// ReSharper disable once UnusedMember.Global using Mediator
public class TransferBetweenAccountsHandler : IRequestHandler<TransferBetweenAccountsCommand, TransactionFullDto>
{
    private readonly IStorageContext _storage;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ITransferFactory _factory;
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _repository;
    private readonly ITransactionWrapper _wrapper;
    private readonly IProducer<TransferCompletedEvent> _producer;
    private readonly IHttpContextAccessor _context;

    public TransferBetweenAccountsHandler(IMapper mapper, IMediator mediator, IStorageContext storage, ITransferFactory factory, IAccountRepository accountRepository, ITransactionRepository repository, ITransactionWrapper wrapper, IProducer<TransferCompletedEvent> producer, IHttpContextAccessor context)
    {
        _mapper = mapper;
        _mediator = mediator;
        _storage = storage;
        _factory = factory;
        _accountRepository = accountRepository;
        _repository = repository;
        _wrapper = wrapper;
        _producer = producer;
        _context = context;
    }

    public async Task<TransactionFullDto> Handle(TransferBetweenAccountsCommand request, CancellationToken cancellationToken)
    {
        var (transaction1, _) =
            await _wrapper.Execute(_ => CreateTransactions(request, cancellationToken), cancellationToken);
        
        return _mapper.Map<TransactionFullDto>(transaction1);
    }

    private async Task<(Transaction, Transaction)> CreateTransactions(TransferBetweenAccountsCommand request, CancellationToken cancellationToken)
    {
        var (accountFrom, accountTo) = await FindByIds(request.AccountId, request.CounterPartyAccountId);
        var transferHandler = _factory.GetTransfer(accountFrom, accountTo, request);

        transferHandler.Validate();

        transferHandler.CreateTransactionForAccountFrom(_mapper.Map<Transaction>(request), _accountRepository);
        transferHandler.CreateTransactionForAccountTo(_mapper.Map<Transaction>(request), _accountRepository);

        transferHandler.SwapTransactionsIds();

        var (transactionFrom, transactionTo) = await transferHandler.SaveToDatabaseAsync(_repository);
        await _storage.SaveChangesAsync(cancellationToken);
        await ProduceAsync(transactionFrom);
        return (transactionFrom, transactionTo);
    }

    private async Task<(Account, Account)> FindByIds(Guid id1, Guid id2)
    {
        var account1 = await FindByIdAccount(id1);
        var account2 = await FindByIdAccount(id2);
        return (account1, account2);
    }

    private async Task<Account> FindByIdAccount(Guid id)
    {
        var query = new FindByIdAccountInternalQuery(id);
        return _mapper.Map<Account>(await _mediator.Send(query));
    }

    private async Task ProduceAsync(Transaction transaction)
    {
        var correlationId = (string)_context.HttpContext!.Items["X-Correlation-ID"]!;
        var meta = MetaCreator.Create(Guid.Parse(correlationId), 
            Guid.Parse("5c14fbc7-77ce-498a-9256-1fef596e9125"));
        var transferEvent = new TransferCompletedEvent(Guid.NewGuid(), DateTime.UtcNow, meta, transaction.Currency)
        {
            Amount = transaction.Sum,
            DestinationAccountId = transaction.CounterPartyAccountId!.Value,
            SourceAccountId = transaction.AccountId,
            TransferId = transaction.Id
        };
        await _producer.ProduceAsync(transferEvent);
    }
}