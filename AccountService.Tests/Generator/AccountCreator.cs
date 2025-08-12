using AccountService.Features.Accounts;
using AccountService.Features.Accounts.CreateAccount;

namespace AccountService.Tests.Generator;

public class AccountCreator
{
    public static Account CreateAccountWithTransaction(Guid id, Guid ownerId, Guid[] transactionsIds, decimal balance = 0, DateTimeOffset createdAt = new(),
        DateTimeOffset? closedAt = null, string currency = "RUB", decimal? percent = null,
        AccountType type = AccountType.Deposit)
    {
        var account = CreateAccount(id, ownerId, balance, createdAt, closedAt, currency, percent, type);
        for (var i = 0; i < transactionsIds.Length; i++)
            account.AccountTransactions.Add(TransactionCreator.CreateTransaction(Guid.NewGuid(), account));
        return account;
    }

    public static Account CreateAccount(Guid id, Guid ownerId, decimal balance = 0, DateTimeOffset createdAt = new(),
        DateTimeOffset? closedAt = null, string currency = "RUB", decimal? percent = null,
        AccountType type = AccountType.Deposit)
    {
        var account = new Account
        {
            Id = id,
            Balance = balance,
            CreatedAt = createdAt,
            ClosedAt = closedAt,
            Currency = currency,
            OwnerId = ownerId,
            Percent = percent,
            Type = type
        };
        

        return account;
    }

    public static CreateAccountCommand CreateCommand(Guid ownerId, decimal balance = 0, string currency = "RUB",
        decimal? percent = null, AccountType type = AccountType.Deposit)
    {
        var command = new CreateAccountCommand
        {
            Balance = balance,
            Currency = currency,
            OwnerId = ownerId,
            Percent = percent,
            Type = type
        };
        return command;
    }
}