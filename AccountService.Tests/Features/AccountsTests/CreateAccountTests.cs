using AccountService.Features.Accounts;
using AccountService.Features.Accounts.CreateAccount;
using AccountService.Features.Users.VerifyUser;
using AccountService.Tests.Asserts;
using AccountService.Tests.Generator;
using AccountService.Utils.Exceptions;
using MediatR;
using Moq;

namespace AccountService.Tests.Features.AccountsTests;

public class CreateAccountTests : AccountTests
{
    private readonly Mock<IMediator> _mediator = new();


    [Fact]
    public async Task CreateAccount_ExistsOwner_Ok()
    {
        //Arrange
        var ownerId = Guid.Parse("3ddd20b3-5445-42af-82e0-cf4a32f002d8");
        var account = AccountCreator.CreateAccount(Guid.Parse("53b77b0a-848c-4e86-a3e5-f8074291b209"), ownerId);
        var command = AccountCreator.CreateCommand(ownerId);
        AccountRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Account>()))
            .ReturnsAsync(account);
        SetupTransaction<Account>(TransactionWrapperMock);
        var token = CancellationToken.None;
        _mediator.Setup(x => x.Send(It.IsAny<VerifyUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new CreateAccountHandler(Mapper, _mediator.Object, StorageMock.Object, AccountRepositoryMock.Object, TransactionWrapperMock.Object);

        //Act
        var result = await handler.Handle(command, token);

        //Assert
        Assert.NotNull(result);
        AccountAssert.Equal(account, result);
    }

    [Fact]
    public async Task CreateAccount_NoExistsOwner_NotFound()
    {
        //Arrange
        var ownerId = Guid.Parse("3ddd20b3-5445-42af-82e0-cf4a32f002d8");
        var account = AccountCreator.CreateAccount(Guid.Parse("53b77b0a-848c-4e86-a3e5-f8074291b209"), ownerId);
        var command = AccountCreator.CreateCommand(ownerId);
        AccountRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Account>()))
            .ReturnsAsync(account);
        SetupTransaction<Account>(TransactionWrapperMock);
        var token = CancellationToken.None;
        _mediator.Setup(x => x.Send(It.IsAny<VerifyUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = new CreateAccountHandler(Mapper, _mediator.Object, StorageMock.Object, AccountRepositoryMock.Object, TransactionWrapperMock.Object);

        //Act

        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, token));
    }
}