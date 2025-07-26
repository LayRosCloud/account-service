using AccountService.Features.Accounts;
using AccountService.Features.Accounts.CreateAccount;
using AccountService.Utils.Time;

namespace AccountService.Tests.Generator;

public class AccountGenerator
{
    public static Account CreateAccount()
    {
        const int countTransaction = 10;
        var account = new Account
        {
            Id = Guid.NewGuid(),
            Balance = 0,
            CreatedAt = TimeUtils.GetTicksFromCurrentDate(),
            ClosedAt = null,
            Currency = "RUB",
            OwnerId = Guid.NewGuid(),
            Percent = null,
            Type = AccountType.Deposit
        };
        for (var i = 0; i < countTransaction; i++)
            account.Transactions.Add(TransactionGenerator.CreateTransaction(account));

        return account;
    }

    public static CreateAccountCommand CreateCommand()
    {
        var command = new CreateAccountCommand
        {
            Balance = 0,
            Currency = "RUB",
            OwnerId = Guid.NewGuid(),
            Percent = null,
            Type = AccountType.Deposit
        };
        return command;
    }
}