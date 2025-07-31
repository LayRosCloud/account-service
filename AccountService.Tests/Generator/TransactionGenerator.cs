using AccountService.Features.Accounts;
using AccountService.Features.Transactions;
using AccountService.Features.Transactions.CreateTransaction;
using AccountService.Features.Transactions.Dto;
using AccountService.Features.Transactions.TransferBetweenAccounts;
using AccountService.Utils.Time;

namespace AccountService.Tests.Generator;

public class TransactionGenerator
{
    public static Transaction CreateTransaction(Account account)
    {
        var random = new Random();

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = account.Id,
            CreatedAt = TimeUtils.GetTicksFromCurrentDate(),
            Currency = account.Currency,
            Sum = (decimal)(random.NextDouble() * 10000),
            Type = TransactionType.Debit,
            Description = "partial withdrawal",
            CounterPartyAccountId = null
        };
        account.Transactions.Add(transaction);
        return transaction;
    }

    public static CreateTransactionCommand CreateCommand(Account account)
    {
        var random = new Random();

        return new CreateTransactionCommand
        {
            AccountId = account.Id,
            Sum = (decimal)(random.NextDouble() * 10000),
            Type = TransactionType.Debit,
            Description = "partial withdrawal"
        };
    }

    public static TransferBetweenAccountsCommand CreateTransfer(Account accountFrom, Account accountTo)
    {
        var random = new Random();

        return new TransferBetweenAccountsCommand
        {
            AccountId = accountFrom.Id,
            CounterPartyAccountId = accountTo.Id,
            Sum = (decimal)(random.NextDouble() * 10000),
            Description = "partial withdrawal"
        };
    }

    public static TransactionFullDto CreateFullTransaction(Account account)
    {
        var random = new Random();

        return new TransactionFullDto
        {
            Id = Guid.NewGuid(),
            AccountId = account.Id,
            CreatedAt = TimeUtils.GetTicksFromCurrentDate(),
            Currency = account.Currency,
            Sum = (decimal)(random.NextDouble() * 10000),
            Type = TransactionType.Debit,
            Description = "partial withdrawal",
            CounterPartyAccountId = null
        };
    }
}