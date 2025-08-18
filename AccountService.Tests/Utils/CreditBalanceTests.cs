using AccountService.Features.Accounts;
using AccountService.Features.Transactions;
using AccountService.Features.Transactions.Utils.Balance;
using AccountService.Tests.Generator;
using Broker.AccountService;
using FluentValidation;

namespace AccountService.Tests.Utils;

public class CreditBalanceTests : BalanceTests
{
    [Fact]
    public void Initialize_CreditBalanceHandlerWithInvalidType_ValidationFail()
    {
        //Arrange
        var credit = AccountCreator.CreateAccount(AccountId, OwnerId, type: AccountType.Deposit);
        //Act

        //Assert
        Assert.Throws<ValidationException>(() => new CreditBalance(credit, TransactionType.Debit));
    }

    [Fact]
    public void Initialize_CreditBalanceHandlerWithValidType_Success()
    {
        //Arrange
        var account = AccountCreator.CreateAccount(AccountId, OwnerId, type: AccountType.Credit);
        //Act
        var credit = new CreditBalance(account, TransactionType.Debit);

        //Assert
        Assert.NotNull(credit);
    }

    [Fact]
    public void Account_NegativeBalance_TransactionCredit_NonClosed_ValidationFail()
    {
        //Arrange
        var account = AccountCreator.CreateAccount(AccountId, OwnerId, balance: -1000M, createdAt: CreatedAt, type: AccountType.Credit);
        var creditBalance = new CreditBalance(account, TransactionType.Credit);
        //Act
        
        //Assert
        Assert.Throws<ValidationException>(() => creditBalance.PerformOperation(500));
    }

    [Fact]
    public void Account_NegativeBalance_TransactionDebit_NonClosed_Success()
    {
        //Arrange
        const decimal amount = 500M;
        var account = AccountCreator.CreateAccount(AccountId, OwnerId, balance: 1000M, createdAt: CreatedAt, type: AccountType.Credit);
        var debitBalance = new CreditBalance(account, TransactionType.Debit);
        var expected = account.Balance + amount;
        //Act
        debitBalance.PerformOperation(amount);

        //Assert
        Assert.Equal(expected, account.Balance);
    }

    [Fact]
    public void Account_NegativeBalance_TransactionDebit_IsClosedAccount_Validation()
    {
        //Arrange
        var closedAt = CreatedAt.AddDays(1);
        var account = AccountCreator.CreateAccount(AccountId, OwnerId, balance: 1000M, createdAt: CreatedAt, closedAt: closedAt, type: AccountType.Credit);

        var creditBalance = new CreditBalance(account, TransactionType.Debit);
        //Act

        //Assert
        Assert.Throws<ValidationException>(() => creditBalance.PerformOperation(500));
    }
}