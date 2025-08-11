using AccountService.Features.Accounts;
using AccountService.Features.Transactions;
using AccountService.Features.Transactions.CreateTransaction;
using AccountService.Features.Transactions.Dto;
using AccountService.Features.Transactions.TransferBetweenAccounts;

namespace AccountService.Tests.Generator;

public class TransactionCreator
{
    public static Transaction CreateTransaction(Guid id, Account account, Account? accountCounterParty = null, decimal sum = 1000,
        DateTimeOffset createdAt = new(), TransactionType type = TransactionType.Debit, string description = "")
    {
        var transaction = new Transaction
        {
            Id = id,
            AccountId = account.Id,
            CreatedAt = createdAt,
            Currency = account.Currency,
            Sum = sum,
            Type = type,
            Description = description,
            CounterPartyAccountId = accountCounterParty?.Id
        };
        account.Transactions.Add(transaction);
        return transaction;
    }

    public static CreateTransactionCommand CreateCommand(Account account, decimal sum = 1000,
        TransactionType type = TransactionType.Debit, string description = "")
    {
        return new CreateTransactionCommand
        {
            AccountId = account.Id,
            Sum = sum,
            Type = type,
            Description = description
        };
    }

    public static TransferBetweenAccountsCommand CreateTransfer(Account accountFrom, Account accountTo, decimal sum = 1000,
        TransactionType type = TransactionType.Credit, string description = "")
    {
        return new TransferBetweenAccountsCommand
        {
            AccountId = accountFrom.Id,
            CounterPartyAccountId = accountTo.Id,
            Sum = sum,
            Type = type,
            Description = description
        };
    }

    public static TransactionFullDto CreateFullTransaction(Guid id, Account account, Account? accountCounterParty = null, decimal sum = 1000,
        DateTimeOffset createdAt = new(), TransactionType type = TransactionType.Debit, string description = "",
        Guid? counterPartyId = null)
    {
        return new TransactionFullDto
        {
            Id = id,
            AccountId = account.Id,
            CreatedAt = createdAt,
            Currency = account.Currency,
            Sum = sum,
            Type = type,
            Description = description,
            CounterPartyAccountId = accountCounterParty?.Id
        };
    }
}