using AccountService.Features.Accounts;
using AccountService.Features.Accounts.FindAllAccounts;
using AccountService.Tests.Asserts;
using AccountService.Tests.Generator;
using Moq;

namespace AccountService.Tests.Features.AccountsTests;

public class FindAllAccountsTests : AccountTests
{
    [Fact]
    public async Task FindAllAccounts_Success()
    {
        //Arrange
        var accounts = new List<Account>(10);
        for (var i = 0; i < accounts.Count; i++)
            accounts.Add(AccountCreator.CreateAccount(Guid.NewGuid(), Guid.NewGuid()));

        AccountRepositoryMock.Setup(x => x.FindAllAsync()).ReturnsAsync(accounts);
        SetupTransaction<List<Account>>(TransactionWrapperMock);
        var query = new FindAllAccountsQuery();
        var handler = new FindAllAccountsHandler(Mapper, AccountRepositoryMock.Object, TransactionWrapperMock.Object);
        
        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        Assert.Equal(result.Count, accounts.Count);
        for (var i = 0; i < result.Count; i++)
            AccountAssert.Equal(accounts[i], result[i]);
    }
}