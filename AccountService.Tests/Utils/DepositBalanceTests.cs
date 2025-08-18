using AccountService.Features.Transactions;
using AccountService.Features.Transactions.Utils.Balance;
using AccountService.Tests.Generator;
using Broker.AccountService;
using FluentValidation;

namespace AccountService.Tests.Utils;

public class DepositBalanceTests : BalanceTests
{
    [Fact]
    public void Initialize_DepositBalanceHandlerWithInvalidType_ValidationFail()
    {
        //Arrange
        var account = AccountCreator.CreateAccount(AccountId, OwnerId, balance: 1000M, createdAt: CreatedAt, type: AccountType.Credit);
        //Act

        //Assert
        Assert.Throws<ValidationException>(() => new DepositBalance(account, TransactionType.Debit));
    }

    [Fact]
    public void Initialize_DepositBalanceHandlerWithValidType_Success()
    {
        //Arrange
        var account = AccountCreator.CreateAccount(AccountId, OwnerId, balance: 1000M, createdAt: CreatedAt, type: AccountType.Deposit);
        //Act
        var deposit = new DepositBalance(account, TransactionType.Debit);

        //Assert
        Assert.NotNull(deposit);
    }

    [Fact]
    public void Account_PositiveBalance_TransactionCredit_NonClosed_Success()
    {
        //Arrange
        const decimal amount = 500M;
        var account = AccountCreator.CreateAccount(AccountId, OwnerId, balance: 1000M, createdAt: CreatedAt, type: AccountType.Deposit);
        var debitBalance = new DepositBalance(account, TransactionType.Credit);
        var expected = account.Balance - amount;
        //Act
        debitBalance.PerformOperation(amount);

        //Assert
        Assert.Equal(expected, account.Balance);
    }

    [Fact]
    public void Account_PositiveBalance_TransactionDebit_NonClosed_Success()
    {
        //Arrange
        const decimal amount = 500M;
        var account = AccountCreator.CreateAccount(AccountId, OwnerId, balance: 1000M, createdAt: CreatedAt, type: AccountType.Deposit);
        var debitBalance = new DepositBalance(account, TransactionType.Debit);
        var expected = account.Balance + amount;
        //Act
        debitBalance.PerformOperation(amount);

        //Assert
        Assert.Equal(expected, account.Balance);
    }

    [Fact]
    public void Account_PositiveBalance_TransactionDebit_IsClosedAccount_Validation()
    {
        //Arrange
        var closedAt = CreatedAt.AddDays(1);
        var account = AccountCreator.CreateAccount(AccountId, OwnerId, balance: 1000M, createdAt: CreatedAt, closedAt: closedAt, type: AccountType.Deposit);
        var paymentProxy = new DepositBalance(account, TransactionType.Debit);
        //Act

        //Assert
        Assert.Throws<ValidationException>(() => paymentProxy.PerformOperation(500));
    }

    [Fact]
    public void Account_PositiveBalance_TransactionCredit_IsClosedAccount_Validation()
    {
        //Arrange
        var closedAt = CreatedAt.AddDays(1);
        var account = AccountCreator.CreateAccount(AccountId, OwnerId, balance: 1000M, createdAt: CreatedAt, closedAt: closedAt, type: AccountType.Deposit);

        var debitBalance = new DepositBalance(account, TransactionType.Credit);
        //Act

        //Assert
        Assert.Throws<ValidationException>(() => debitBalance.PerformOperation(500));
    }

    [Fact]
    public void Account_ZeroBalance_TransactionCredit_NonClosed_ValidationFail()
    {
        //Arrange
        var account = AccountCreator.CreateAccount(AccountId, OwnerId, balance: 0M, createdAt: CreatedAt, type: AccountType.Deposit);
        var debitBalance = new DepositBalance(account, TransactionType.Credit);
        //Act

        //Assert
        Assert.Throws<ValidationException>(() => debitBalance.PerformOperation(500));
    }

    [Fact]
    public void Account_ZeroBalance_TransactionDebit_NonClosed_Success()
    {
        //Arrange
        const decimal amount = 500M;
        var account = AccountCreator.CreateAccount(AccountId, OwnerId, balance: 0M, createdAt: CreatedAt, type: AccountType.Deposit);
        var debitBalance = new DepositBalance(account, TransactionType.Debit);
        var expected = account.Balance + amount;
        //Act
        debitBalance.PerformOperation(amount);

        //Assert
        Assert.Equal(expected, account.Balance);
    }
}