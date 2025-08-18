using AccountService.Features.Transactions;
using AccountService.Features.Transactions.Utils.Balance;
using AccountService.Tests.Generator;
using Broker.AccountService;

namespace AccountService.Tests.Utils;

public class BalanceFactoryTests : BalanceTests
{
    [Fact]
    public void GetDepositBalance_Success()
    {
        //Arrange
        var factory = new BalanceFactory();
        var account = AccountCreator.CreateAccount(AccountId, OwnerId, type: AccountType.Deposit);
        
        //Act
        var balance = factory.GetBalanceHandler(account, TransactionType.Debit);

        //Assert
        Assert.True(balance is DepositBalance);
    }

    [Fact]
    public void GetCreditBalance_Success()
    {
        //Arrange
        var factory = new BalanceFactory();
        var account = AccountCreator.CreateAccount(AccountId, OwnerId, type: AccountType.Credit);

        //Act
        var balance = factory.GetBalanceHandler(account, TransactionType.Credit);

        //Assert
        Assert.True(balance is CreditBalance);
    }

    [Fact]
    public void GetCheckingBalance_Success()
    {
        //Arrange
        var factory = new BalanceFactory();
        var account = AccountCreator.CreateAccount(AccountId, OwnerId, type: AccountType.Checking);

        //Act
        var balance = factory.GetBalanceHandler(account, TransactionType.Credit);

        //Assert
        Assert.True(balance is CheckingBalance);
    }
}