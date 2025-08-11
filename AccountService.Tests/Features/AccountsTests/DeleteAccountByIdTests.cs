using AccountService.Features.Accounts;
using AccountService.Features.Accounts.DeleteAccount;
using AccountService.Tests.Generator;
using AccountService.Utils.Exceptions;
using Moq;

namespace AccountService.Tests.Features.AccountsTests;

public class DeleteAccountByIdTests : AccountTests
{
    [Fact]
    public async Task DeleteAccountIfExistsId_Ok()
    {
        //Arrange
        var id = Guid.Parse("d5aa9996-7fe9-4584-b23c-4a1899f81e04");
        var account = AccountCreator.CreateAccount(id, Guid.NewGuid());
        AccountRepositoryMock.Setup(x => x.FindByIdAsync(id, false))
            .ReturnsAsync(account);
        SetupTransaction<Account>(TransactionWrapperMock);

        var command = new DeleteAccountCommand(id);
        var handler = new DeleteAccountHandler(StorageMock.Object, AccountRepositoryMock.Object, TransactionWrapperMock.Object);

        //Act
        var result = await handler.Handle(command, default);
        var account1 = await AccountRepositoryMock.Object.FindByIdAsync(id);
        //Assert
        Assert.NotNull(result);
        Assert.NotNull(account1!.ClosedAt);
    }

    [Fact]
    public async Task DeleteAccountIfNotExistsId_NotFound()
    {
        //Arrange
        var id = Guid.Parse("d5aa9996-7fe9-4584-b23c-4a1899f81e04");

        var command = new DeleteAccountCommand(id);
        var handler = new DeleteAccountHandler(StorageMock.Object, AccountRepositoryMock.Object, TransactionWrapperMock.Object);

        SetupTransaction<Account>(TransactionWrapperMock);

        //Act

        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, default));
    }
}