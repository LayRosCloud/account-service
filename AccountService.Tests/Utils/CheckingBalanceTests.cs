using AccountService.Features.Accounts;
using AccountService.Features.Transactions.Utils.Balance;
using AccountService.Tests.Generator;
using Broker.AccountService;
using FluentValidation;

namespace AccountService.Tests.Utils;

public class CheckingBalanceTests : BalanceTests
{

    [Fact]
    public void Initialize_CheckingBalanceHandlerWithInvalidType_ValidationFail()
    {
        //Arrange
        var account = AccountCreator.CreateAccount(AccountId, OwnerId, type: AccountType.Deposit);
        //Act

        //Assert
        Assert.Throws<ValidationException>(() => new CheckingBalance(account));
    }

    [Fact]
    public void Initialize_CheckingBalanceHandlerWithValidType_Success()
    {
        //Arrange
        var account = AccountCreator.CreateAccount(AccountId, OwnerId, type: AccountType.Checking);
        //Act
        var credit = new CheckingBalance(account);

        //Assert
        Assert.NotNull(credit);
    }

    [Fact]
    public void Account_NegativeBalance_TransactionCredit_NonClosed_ValidationFail()
    {
        //Arrange
        var account = AccountCreator.CreateAccount(AccountId, OwnerId, type: AccountType.Checking);
        var checkingBalance = new CheckingBalance(account);
        //Act

        //Assert
        Assert.Throws<ValidationException>(() => checkingBalance.PerformOperation(500));
    }

    [Fact]
    public void Account_NegativeBalance_TransactionDebit_NonClosed_ValidationFail()
    {
        //Arrange
        var account = AccountCreator.CreateAccount(AccountId, OwnerId, type: AccountType.Checking);
        var checkingBalance = new CheckingBalance(account);
        //Act

        //Assert
        Assert.Throws<ValidationException>(() => checkingBalance.PerformOperation(500));
    }
}