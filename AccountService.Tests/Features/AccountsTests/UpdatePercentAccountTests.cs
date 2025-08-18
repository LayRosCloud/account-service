using AccountService.Features.Accounts;
using AccountService.Features.Accounts.UpdatePercentAccount;
using AccountService.Tests.Generator;
using AccountService.Utils.Exceptions;
using Broker.AccountService;
using FluentValidation;
using Moq;

namespace AccountService.Tests.Features.AccountsTests;

public class UpdatePercentAccountTests : AccountTests
{
    [Fact]
    public async Task UpdateAccount_ExistsAccount_Ok()
    {
        //Arrange
        var ownerId = Guid.Parse("3ddd20b3-5445-42af-82e0-cf4a32f002d8");
        var accountFindById = AccountCreator.CreateAccount(Guid.Parse("53b77b0a-848c-4e86-a3e5-f8074291b209"), ownerId);
        var accountUpdate = AccountCreator.CreateAccount(Guid.Parse("53b77b0a-848c-4e86-a3e5-f8074291b209"), ownerId, percent: 6.5M);
        var command = new UpdateAccountPercentCommand
        {
            Id = accountFindById.Id,
            Percent = accountUpdate.Percent
        };
        AccountRepositoryMock.Setup(x => x.FindByIdAsync(accountFindById.Id, false))
            .ReturnsAsync(accountFindById);
        AccountRepositoryMock.Setup(x => x.Update(It.IsAny<Account>()))
            .Returns(accountUpdate);
        SetupTransaction<Account>(TransactionWrapperMock);
        var token = CancellationToken.None;

        var handler = new UpdateAccountPercentHandler(Mapper, StorageMock.Object, AccountRepositoryMock.Object, TransactionWrapperMock.Object);

        //Act
        var result = await handler.Handle(command, token);

        //Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Percent);
        Assert.Equal(command.Percent, result.Percent);
    }

    [Fact]
    public async Task UpdateAccount_ExistsAccount_NotFound()
    {
        //Arrange
        var ownerId = Guid.Parse("3ddd20b3-5445-42af-82e0-cf4a32f002d8");
        var accountFindById = AccountCreator.CreateAccount(Guid.Parse("53b77b0a-848c-4e86-a3e5-f8074291b209"), ownerId);
        var command = new UpdateAccountPercentCommand
        {
            Id = accountFindById.Id,
            Percent = null
        };
        AccountRepositoryMock.Setup(x => x.FindByIdAsync(accountFindById.Id, false))
            .ReturnsAsync((Account?)null);

        SetupTransaction<Account>(TransactionWrapperMock);
        var token = CancellationToken.None;

        var handler = new UpdateAccountPercentHandler(Mapper, StorageMock.Object, AccountRepositoryMock.Object, TransactionWrapperMock.Object);

        //Act

        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, token));
    }

    [Fact]
    public async Task UpdateAccount_AccountCreditTypeAndPercentNotNull_Success()
    {
        //Arrange
        var ownerId = Guid.Parse("3ddd20b3-5445-42af-82e0-cf4a32f002d8");
        var accountFindById = AccountCreator.CreateAccount(Guid.Parse("53b77b0a-848c-4e86-a3e5-f8074291b209"), ownerId, type:AccountType.Credit);
        var command = new UpdateAccountPercentCommand
        {
            Id = accountFindById.Id,
            Percent = 6.5M
        };
        AccountRepositoryMock.Setup(x => x.FindByIdAsync(accountFindById.Id, false))
            .ReturnsAsync(accountFindById);

        SetupTransaction<Account>(TransactionWrapperMock);
        var token = CancellationToken.None;

        var handler = new UpdateAccountPercentHandler(Mapper, StorageMock.Object, AccountRepositoryMock.Object, TransactionWrapperMock.Object);

        //Act
        var result = await handler.Handle(command, token);

        //Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Percent);
        Assert.Equal(command.Percent, result.Percent);
    }

    [Fact]
    public async Task UpdateAccount_AccountCreditTypeAndPercentNullable_ValidationFailure()
    {
        //Arrange
        var ownerId = Guid.Parse("3ddd20b3-5445-42af-82e0-cf4a32f002d8");
        var accountFindById = AccountCreator.CreateAccount(Guid.Parse("53b77b0a-848c-4e86-a3e5-f8074291b209"), ownerId, type: AccountType.Credit);
        var command = new UpdateAccountPercentCommand
        {
            Id = accountFindById.Id,
            Percent = null
        };
        AccountRepositoryMock.Setup(x => x.FindByIdAsync(accountFindById.Id, false))
            .ReturnsAsync(accountFindById);

        SetupTransaction<Account>(TransactionWrapperMock);
        var token = CancellationToken.None;

        var handler = new UpdateAccountPercentHandler(Mapper, StorageMock.Object, AccountRepositoryMock.Object, TransactionWrapperMock.Object);

        //Act

        //Assert
        await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, token));
    }
}