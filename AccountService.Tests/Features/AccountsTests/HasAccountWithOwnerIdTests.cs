using AccountService.Features.Accounts.HasAccountWithCounterParty;
using Moq;

namespace AccountService.Tests.Features.AccountsTests;

public class HasAccountWithOwnerIdTests : AccountTests
{
    [Fact]
    public async Task HasAccountWithOwnerId_ExistsAccountIdAndOwnerId_Success()
    {
        //Arrange
        var accountId = Guid.Parse("53b77b0a-848c-4e86-a3e5-f8074291b209");
        var ownerId = Guid.Parse("f344269c-d75d-4c75-b26d-cc670e05a8f3");

        AccountRepositoryMock
            .Setup(x => x.HasAccountAndOwnerAsync(accountId, ownerId))
            .ReturnsAsync(true);
        SetupTransaction<bool>(TransactionWrapperMock);
        var command = new HasAccountWithCounterPartyCommand(ownerId, accountId);
        var handler =
            new HasAccountWithCounterPartyHandler(AccountRepositoryMock.Object, TransactionWrapperMock.Object);
        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.Equal(true, result);
    }

    [Fact]
    public async Task HasAccountWithOwnerId_NotExistsAccountIdAndOwnerId_Success()
    {
        //Arrange
        var accountId = Guid.Parse("53b77b0a-848c-4e86-a3e5-f8074291b209");
        var ownerId = Guid.Parse("f344269c-d75d-4c75-b26d-cc670e05a8f3");

        AccountRepositoryMock
            .Setup(x => x.HasAccountAndOwnerAsync(accountId, ownerId))
            .ReturnsAsync(false);
        SetupTransaction<bool>(TransactionWrapperMock);
        var command = new HasAccountWithCounterPartyCommand(ownerId, accountId);
        var handler =
            new HasAccountWithCounterPartyHandler(AccountRepositoryMock.Object, TransactionWrapperMock.Object);
        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.Equal(false, result);
    }
}