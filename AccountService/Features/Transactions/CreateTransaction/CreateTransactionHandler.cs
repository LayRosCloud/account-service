using AccountService.Features.Accounts;
using AccountService.Features.Transactions.Dto;
using AccountService.Features.Transactions.Utils.Balance;
using AccountService.Utils.Data;
using AccountService.Utils.Exceptions;
using AutoMapper;
using MediatR;

namespace AccountService.Features.Transactions.CreateTransaction;

// ReSharper disable once UnusedMember.Global using Mediator
public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, TransactionFullDto>
{
    private readonly IStorageContext _storage;
    private readonly IMapper _mapper;
    private readonly ITransactionRepository _repository;
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionWrapper _wrapper;

    public CreateTransactionHandler(IMapper mapper, IStorageContext storage, ITransactionRepository repository, IAccountRepository accountRepository, ITransactionWrapper wrapper)
    {
        _mapper = mapper;
        _storage = storage;
        _repository = repository;
        _accountRepository = accountRepository;
        _wrapper = wrapper;
    }

    public async Task<TransactionFullDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var result =
            await _wrapper.Execute(_ => CreateTransactionAsync(request, cancellationToken), cancellationToken);
        return _mapper.Map<TransactionFullDto>(result);
    }

    public async Task<Transaction> CreateTransactionAsync(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var account = await FindAccountById(request.AccountId);

        var transaction = _mapper.Map<Transaction>(request);
        SetDefaultSettingTransaction(account, transaction);

        var proxy = new PaymentBalance(transaction, account);
        proxy.ExecuteTransactionAsync(_accountRepository);

        var result = await _repository.CreateAsync(transaction);
        await _storage.SaveChangesAsync(cancellationToken);
        return result;
    }

    private async Task<Account> FindAccountById(Guid id)
    {
        var account = await _accountRepository.FindByIdAsync(id);
        if (account == null)
            throw ExceptionUtils.GetNotFoundException("Account", id);
        return account;
    }

    private static void SetDefaultSettingTransaction(Account account, Transaction transaction)
    {
        transaction.Currency = account.Currency;
        transaction.Id = Guid.NewGuid();
    }
}