using System.ComponentModel.DataAnnotations;
using AccountService.Features.Accounts;
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
    private readonly DatabaseContext _databaseContext = DatabaseContext.Instance;
    private readonly IMapper _mapper;

    public TransferBetweenAccountsHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Task<TransactionFullDto> Handle(TransferBetweenAccountsCommand request, CancellationToken cancellationToken)
    {
        var accountFrom = FindByIdAccount(request.AccountId);
        var accountTo = FindByIdAccount(request.CounterPartyAccountId);

        if (accountFrom == null || accountTo == null)
            throw ExceptionUtils.GetNotFoundException("Accounts", $"{request.AccountId} {request.CounterPartyAccountId}");

        CheckAccountConditions(accountFrom, accountTo);

        var transaction = _mapper.Map<Transaction>(request);
        SetDefaultSettingsTransaction(transaction, accountFrom);

        var proxy = new PaymentProxy(transaction, accountFrom, accountTo);
        proxy.ExecuteTransaction();

        accountFrom.Transactions.Add(transaction);
        accountTo.Transactions.Add(transaction);
        _databaseContext.Transactions.Add(transaction);

        return Task.FromResult(_mapper.Map<TransactionFullDto>(transaction));
    }

    private static void CheckAccountConditions(Account accountFrom, Account accountTo)
    {
        if (!accountFrom.Currency.Equals(accountTo.Currency))
            throw new ValidationException("Currency accounts is different");
    }

    private Account? FindByIdAccount(Guid id)
    {
        return _databaseContext.Accounts.SingleOrDefault(acc => acc.Id == id);
    }

    private static void SetDefaultSettingsTransaction(Transaction transaction, Account account)
    {
        transaction.Currency = account.Currency;
        transaction.Type = TransactionType.Debit;
        transaction.CreatedAt = TimeUtils.GetTicksFromCurrentDate();
        transaction.Id = new Guid();
    }
}