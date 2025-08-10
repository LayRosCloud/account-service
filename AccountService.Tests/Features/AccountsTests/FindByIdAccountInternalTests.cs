using AccountService.Features.Accounts;
using AccountService.Features.Accounts.FindByIdAccount.Internal;
using AccountService.Tests.Asserts;
using AccountService.Tests.Generator;
using AccountService.Utils.Exceptions;
using Moq;

namespace AccountService.Tests.Features.AccountsTests;

public class FindByIdAccountInternalTests : AccountTests
{

    [Fact]
    public async Task FindByIdAccountIdInternal_Ok()
    {
        //Arrange
        var id = Guid.Parse("d5aa9996-7fe9-4584-b23c-4a1899f81e04");
        var account = AccountCreator.CreateAccount(id, Guid.NewGuid());
        AccountRepositoryMock.Setup(x => x.FindByIdAsync(id, false))
            .ReturnsAsync(account);

        var command = new FindByIdAccountInternalQuery(id);
        var handler = new FindByIdAccountInternalHandler(TransactionWrapperMock.Object, AccountRepositoryMock.Object);

        SetupTransaction<Account>(TransactionWrapperMock);

        //Act
        var result = await handler.Handle(command, default);

        //Assert
        Assert.NotNull(result);
        AccountAssert.Equal(account, result);
    }

    [Fact]
    public async Task FindByIdAccountIdInternal_NotFound()
    {
        //Arrange
        var command = new FindByIdAccountInternalQuery(Guid.Empty);
        var handler = new FindByIdAccountInternalHandler(TransactionWrapperMock.Object, AccountRepositoryMock.Object);
        SetupTransaction<Account>(TransactionWrapperMock);

        //Act

        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }

    
}