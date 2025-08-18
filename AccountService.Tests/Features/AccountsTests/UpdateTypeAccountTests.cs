using AccountService.Features.Accounts;
using AccountService.Features.Accounts.FindByIdAccount.Internal;
using AccountService.Features.Accounts.UpdateTypeAccount;
using AccountService.Tests.Asserts;
using AccountService.Tests.Generator;
using Broker.AccountService;
using FluentValidation;
using MediatR;
using Moq;

namespace AccountService.Tests.Features.AccountsTests;

public class UpdateTypeAccountTests : AccountTests
{
    private readonly Mock<IMediator> _mediatorMock = new();

    [Fact]
    public async Task UpdateTypeAccount_ExistsAccount_Ok()
    {
        //Arrange
        var ownerId = Guid.Parse("3ddd20b3-5445-42af-82e0-cf4a32f002d8");
        var accountFindById = AccountCreator.CreateAccount(Guid.Parse("53b77b0a-848c-4e86-a3e5-f8074291b209"), ownerId, type: AccountType.Credit);
        var accountUpdate = AccountCreator.CreateAccount(Guid.Parse("53b77b0a-848c-4e86-a3e5-f8074291b209"), ownerId, type: AccountType.Checking);
        var command = new UpdateAccountTypeCommand(accountUpdate.Type)
        {
            AccountId = accountFindById.Id,
        };
        _mediatorMock.Setup(x => x.Send(It.IsAny<FindByIdAccountInternalQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(accountFindById);
        AccountRepositoryMock.Setup(x => x.Update(It.IsAny<Account>()))
            .Returns(accountUpdate);
        SetupTransaction<Account>(TransactionWrapperMock);
        var token = CancellationToken.None;

        var handler = new UpdateAccountTypeHandler(Mapper, _mediatorMock.Object, AccountRepositoryMock.Object, StorageMock.Object, TransactionWrapperMock.Object);

        //Act
        var result = await handler.Handle(command, token);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(command.Type, result.Type);
        AccountAssert.Equal(accountUpdate, accountUpdate);
    }

    [Fact]
    public async Task UpdateTypeAccount_CheckingToDepositWithNegativeBalance_ValidationFailure()
    {
        //Arrange
        var ownerId = Guid.Parse("3ddd20b3-5445-42af-82e0-cf4a32f002d8");
        var accountFindById = AccountCreator.CreateAccount(Guid.Parse("53b77b0a-848c-4e86-a3e5-f8074291b209"), ownerId, type: AccountType.Checking, balance: -10M);
        var command = new UpdateAccountTypeCommand(AccountType.Deposit)
        {
            AccountId = accountFindById.Id,
        };
        _mediatorMock.Setup(x => x.Send(It.IsAny<FindByIdAccountInternalQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(accountFindById);


        SetupTransaction<Account>(TransactionWrapperMock);
        var token = CancellationToken.None;

        var handler = new UpdateAccountTypeHandler(Mapper, _mediatorMock.Object, AccountRepositoryMock.Object, StorageMock.Object, TransactionWrapperMock.Object);

        //Act

        //Assert
        await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, token));
    }

    [Fact]
    public async Task UpdateTypeAccount_CheckingToDepositWithPositiveBalance_Success()
    {
        //Arrange
        var ownerId = Guid.Parse("3ddd20b3-5445-42af-82e0-cf4a32f002d8");
        var accountFindById = AccountCreator.CreateAccount(Guid.Parse("53b77b0a-848c-4e86-a3e5-f8074291b209"), ownerId, type: AccountType.Checking, balance: 10M);
        var accountUpdate = AccountCreator.CreateAccount(Guid.Parse("53b77b0a-848c-4e86-a3e5-f8074291b209"), ownerId,
            type: AccountType.Deposit, balance: 10M);
        var command = new UpdateAccountTypeCommand(AccountType.Deposit)
        {
            AccountId = accountFindById.Id,
        };
        _mediatorMock.Setup(x => x.Send(It.IsAny<FindByIdAccountInternalQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(accountFindById);
        AccountRepositoryMock.Setup(x => x.Update(It.IsAny<Account>()))
            .Returns(accountUpdate);
        SetupTransaction<Account>(TransactionWrapperMock);

        var token = CancellationToken.None;

        var handler = new UpdateAccountTypeHandler(Mapper, _mediatorMock.Object, AccountRepositoryMock.Object, StorageMock.Object, TransactionWrapperMock.Object);

        //Act
        var result = await handler.Handle(command, token);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(command.Type, result.Type);
        AccountAssert.Equal(accountUpdate, result);
    }

    [Fact]
    public async Task UpdateTypeAccount_CheckingToCreditWithPercentNotNull_Success()
    {
        //Arrange
        var ownerId = Guid.Parse("3ddd20b3-5445-42af-82e0-cf4a32f002d8");
        var accountFindById = AccountCreator.CreateAccount(Guid.Parse("53b77b0a-848c-4e86-a3e5-f8074291b209"), ownerId, type: AccountType.Checking, percent: 6.7M);
        var accountUpdate = AccountCreator.CreateAccount(Guid.Parse("53b77b0a-848c-4e86-a3e5-f8074291b209"), ownerId, type: AccountType.Credit);
        var command = new UpdateAccountTypeCommand(AccountType.Credit)
        {
            AccountId = accountFindById.Id,
        };
        _mediatorMock.Setup(x => x.Send(It.IsAny<FindByIdAccountInternalQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(accountFindById);
        AccountRepositoryMock.Setup(x => x.Update(It.IsAny<Account>()))
            .Returns(accountUpdate);

        SetupTransaction<Account>(TransactionWrapperMock);
        var token = CancellationToken.None;

        var handler = new UpdateAccountTypeHandler(Mapper, _mediatorMock.Object, AccountRepositoryMock.Object, StorageMock.Object, TransactionWrapperMock.Object);

        //Act
        var result = await handler.Handle(command, token);
        //Assert
        Assert.NotNull(result);
        Assert.Equal(command.Type, result.Type);
        AccountAssert.Equal(accountUpdate, result);
    }

    [Fact]
    public async Task UpdateTypeAccount_CreditAndDeposit_ValidationFailure()
    {
        //Arrange
        var ownerId = Guid.Parse("3ddd20b3-5445-42af-82e0-cf4a32f002d8");
        var accountFindById = AccountCreator.CreateAccount(Guid.Parse("53b77b0a-848c-4e86-a3e5-f8074291b209"), ownerId, type: AccountType.Deposit);
        var command = new UpdateAccountTypeCommand(AccountType.Credit)
        {
            AccountId = accountFindById.Id,
        };
        _mediatorMock.Setup(x => x.Send(It.IsAny<FindByIdAccountInternalQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(accountFindById);


        SetupTransaction<Account>(TransactionWrapperMock);
        var token = CancellationToken.None;

        var handler = new UpdateAccountTypeHandler(Mapper, _mediatorMock.Object, AccountRepositoryMock.Object, StorageMock.Object, TransactionWrapperMock.Object);

        //Act

        //Assert
        await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, token));
    }

    [Fact]
    public async Task UpdateTypeAccount_DepositAndCredit_ValidationFailure()
    {
        //Arrange
        var ownerId = Guid.Parse("3ddd20b3-5445-42af-82e0-cf4a32f002d8");
        var accountFindById = AccountCreator.CreateAccount(Guid.Parse("53b77b0a-848c-4e86-a3e5-f8074291b209"), ownerId, type: AccountType.Deposit);
        var command = new UpdateAccountTypeCommand(AccountType.Credit)
        {
            AccountId = accountFindById.Id,
        };
        _mediatorMock.Setup(x => x.Send(It.IsAny<FindByIdAccountInternalQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(accountFindById);


        SetupTransaction<Account>(TransactionWrapperMock);
        var token = CancellationToken.None;

        var handler = new UpdateAccountTypeHandler(Mapper, _mediatorMock.Object, AccountRepositoryMock.Object, StorageMock.Object, TransactionWrapperMock.Object);

        //Act

        //Assert
        await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, token));
    }
}