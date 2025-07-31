using System.ComponentModel.DataAnnotations;
using AccountService.Features.Accounts;
using AccountService.Features.Accounts.FindByIdAccount.Internal;
using AccountService.Features.Transactions.Dto;
using AccountService.Features.Transactions.Utils.Balance;
using AccountService.Utils.Data;
using AccountService.Utils.Exceptions;
using AccountService.Utils.Time;
using AutoMapper;
using MediatR;

namespace AccountService.Features.Transactions.TransferBetweenAccounts;

public class TransferBetweenAccountsHandler : IRequestHandler<TransferBetweenAccountsCommand, TransactionFullDto>
{
    private readonly IDatabaseContext _database;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public TransferBetweenAccountsHandler(IMapper mapper, IMediator mediator, IDatabaseContext database)
    {
        _mapper = mapper;
        _mediator = mediator;
        _database = database;
    }

    public Task<TransactionFullDto> Handle(TransferBetweenAccountsCommand request, CancellationToken cancellationToken)
    {
        var accountFrom = FindByIdAccount(request.AccountId);
        var accountTo = FindByIdAccount(request.CounterPartyAccountId);

        if (accountFrom == null || accountTo == null)
            throw ExceptionUtils.GetNotFoundException("Accounts",
                $"{request.AccountId} {request.CounterPartyAccountId}");

        CheckAccountConditions(accountFrom, accountTo);

        var transactionFrom = CreateTransaction(request, accountFrom);
        var transactionTo = CreateTransaction(request, accountTo,
            transactionFrom.Type == TransactionType.Debit ? TransactionType.Credit : TransactionType.Debit);
        transactionTo.AccountId = transactionFrom.CounterPartyAccountId!.Value;
        transactionTo.CounterPartyAccountId = transactionFrom.AccountId;
        AddTransactionToDatabase(transactionFrom, accountFrom);
        AddTransactionToDatabase(transactionTo, accountTo);
        return Task.FromResult(_mapper.Map<TransactionFullDto>(transactionFrom));
    }

    private Transaction CreateTransaction(TransferBetweenAccountsCommand request, Account account,
        TransactionType? type = null)
    {
        var transaction = _mapper.Map<Transaction>(request);
        if (type != null)
            transaction.Type = type.Value;
        SetDefaultSettingsTransaction(transaction, account);
        var proxyFrom = new PaymentProxy(transaction, account);

        proxyFrom.ExecuteTransaction();

        return transaction;
    }

    private void AddTransactionToDatabase(Transaction transaction, Account account)
    {
        account.Transactions.Add(transaction);
        _database.Transactions.Add(transaction);
    }

    private static void CheckAccountConditions(Account accountFrom, Account accountTo)
    {
        if (!accountFrom.Currency.Equals(accountTo.Currency))
            throw new ValidationException("Currency accounts is different");
    }

    private Account FindByIdAccount(Guid id)
    {
        var query = new FindByIdAccountInternalQuery(id);
        return _mapper.Map<Account>(_mediator.Send(query).Result);
    }

    private static void SetDefaultSettingsTransaction(Transaction transaction, Account account)
    {
        transaction.Currency = account.Currency;
        transaction.CreatedAt = TimeUtils.GetTicksFromCurrentDate();
        transaction.Id = Guid.NewGuid();
    }
}