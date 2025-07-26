using AccountService.Features.Accounts;
using AccountService.Features.Accounts.CreateAccount;
using AccountService.Features.Accounts.Dto;

namespace AccountService.Tests.Utils;

public class AccountAssert
{
    public static void AssertAccountAndShortDto(Account account, AccountResponseShortDto destination)
    {
        Assert.NotNull(destination);
        Assert.Equal(account.Id, destination.Id);
        Assert.Equal(account.Type, destination.Type);
        Assert.Equal(account.OwnerId, destination.OwnerId);
        Assert.Equal(account.Balance, destination.Balance);
        Assert.Equal(account.CreatedAt, destination.CreatedAt);
        Assert.Equal(account.ClosedAt, destination.ClosedAt);
        Assert.Equal(account.Percent, destination.Percent);
        Assert.Equal(account.Currency, destination.Currency);
    }

    public static void AssertCreateCommandAndEntity(CreateAccountCommand command, Account account)
    {
        Assert.NotNull(account);
        Assert.Equal(command.Balance, account.Balance);
        Assert.Equal(command.Type, account.Type);
        Assert.Equal(command.OwnerId, account.OwnerId);
        Assert.Equal(command.Percent, account.Percent);
        Assert.Equal(command.Currency, account.Currency);
    }

    public static void AssertAccountAndFullDto(Account account, AccountResponseFullDto destination)
    {
        Assert.NotNull(destination);
        Assert.Equal(account.Id, destination.Id);
        Assert.Equal(account.Type, destination.Type);
        Assert.Equal(account.OwnerId, destination.OwnerId);
        Assert.Equal(account.Balance, destination.Balance);
        Assert.Equal(account.CreatedAt, destination.CreatedAt);
        Assert.Equal(account.ClosedAt, destination.ClosedAt);
        Assert.Equal(account.Percent, destination.Percent);
        Assert.Equal(account.Currency, destination.Currency);
        for (var i = 0; i < account.Transactions.Count; i++)
            TransactionAssert.AssertTransactions(account.Transactions[i], destination.Transactions[i]);
    }
}