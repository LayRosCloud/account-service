using AccountService.Features.Accounts;
using AccountService.Features.Transactions.TransferBetweenAccounts;
using AccountService.Features.Transactions.Utils.Balance;
using AccountService.Utils.Data;
using AccountService.Utils.Exceptions;
using AccountService.Utils.Time;
using FluentValidation;

namespace AccountService.Features.Transactions.Utils.Transfer;

public class TransferHandler : ITransfer
{
    private readonly AccountTransaction _accountFrom;
    private readonly AccountTransaction _accountTo;
    private readonly TransferBetweenAccountsCommand _request;

    public TransferHandler(Account accountFrom, Account accountTo, TransferBetweenAccountsCommand request)
    {
        _accountFrom = new AccountTransaction(accountFrom);
        _accountTo = new AccountTransaction(accountTo);
        _request = request;
    }


    public void Validate()
    {
        if (_accountFrom.Account == null || _accountTo.Account == null)
            throw ExceptionUtils.GetNotFoundException("Accounts",
                $"{_request.AccountId} {_request.CounterPartyAccountId}");

        if (!_accountFrom.Account.Currency.Equals(_accountTo.Account.Currency))
            throw new ValidationException("Currency accounts is different");
    }

    public void CreateTransactionForAccountFrom(Transaction transaction)
    {
        CreateTransaction(transaction);
    }

    public void CreateTransactionForAccountTo(Transaction transaction)
    {
        transaction.Type = TransactionType.Debit == transaction.Type ? TransactionType.Credit : TransactionType.Debit;
        CreateTransaction(transaction, true);
    }

    public void SwapTransactionsIds()
    {
        _accountTo.Transaction.AccountId = _accountFrom.Transaction.CounterPartyAccountId!.Value;
        _accountTo.Transaction.CounterPartyAccountId = _accountFrom.Transaction.AccountId;
    }

    private void CreateTransaction(Transaction transaction, bool isAccountTo = false)
    {
        var account = isAccountTo ? _accountTo : _accountFrom;
        SetDefaultSettingsTransaction(transaction, account.Account.Currency);
        var balance = new PaymentBalance(transaction, account.Account);
        balance.ExecuteTransaction();
        account.Transaction = transaction;
    }

    public (Transaction, Transaction) SaveToDatabase(IDatabaseContext context)
    {
        _accountFrom.AddToDatabase(context);
        _accountTo.AddToDatabase(context);
        return (_accountFrom.Transaction, _accountTo.Transaction);
    }

    private static void SetDefaultSettingsTransaction(Transaction transaction, string currency)
    {
        transaction.Currency = currency;
        transaction.CreatedAt = TimeUtils.GetTicksFromCurrentDate();
        transaction.Id = Guid.NewGuid();
    }
}