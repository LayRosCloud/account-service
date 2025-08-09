using AccountService.Features.Accounts;
using AccountService.Features.Transactions;
using AccountService.Features.Transactions.Dto;
using AccountService.Tests.Asserts;
using AccountService.Tests.Generator;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AccountService.Tests.Mapper;

public class TransactionMapperTests
{
    private readonly IMapper _mapper;
    private readonly Guid _transactionId = Guid.Parse("0aeee6d5-ccb5-4633-803a-c15288805589");

    public TransactionMapperTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddAutoMapper(x => x.AddProfile(typeof(TransactionMapper)));

        var serviceProvider = serviceCollection.BuildServiceProvider();
        _mapper = serviceProvider.GetRequiredService<IMapper>();
    }

    [Fact]
    public void MapToFullDto_Success()
    {
        //Arrange
        var account = CreateTemplateAccount();
        var transaction =
            TransactionCreator.CreateTransaction(_transactionId, account);

        //Act
        var destination = _mapper.Map<TransactionFullDto>(transaction);

        //Assert
        TransactionAssert.AssertTransactions(transaction, destination);
    }

    [Fact]
    public void MapToEntityFromCreateCommand_Success()
    {
        //Arrange
        var account = CreateTemplateAccount();
        var transaction = TransactionCreator.CreateCommand(account);

        //Act
        var destination = _mapper.Map<Transaction>(transaction);

        //Assert
        TransactionAssert.AssertTransactions(destination, transaction);
    }

    [Fact]
    public void MapToTransactionFromTransferObject_Success()
    {
        //Arrange
        var accountFrom = CreateTemplateAccount();
        var accountTo = AccountCreator.CreateAccount(Guid.Parse("09375faa-50c9-4a92-9270-3895e077a342"), Guid.Parse("03059486-cfcb-42ba-96a7-9a8844bb6984"));
        var transaction = TransactionCreator.CreateTransfer(accountFrom, accountTo);

        //Act
        var destination = _mapper.Map<Transaction>(transaction);

        //Assert
        TransactionAssert.AssertTransactions(destination, transaction);
    }

    [Fact]
    public void MapToTransactionFullDto_Success()
    {
        //Arrange
        var account = CreateTemplateAccount();
        var transaction = TransactionCreator.CreateFullTransaction(_transactionId, account);

        //Act
        var destination = _mapper.Map<Transaction>(transaction);

        //Assert
        TransactionAssert.AssertTransactions(destination, transaction);
    }

    private static Account CreateTemplateAccount()
    {
        var ownerId = Guid.Parse("defd9e37-b69d-4813-8bf7-6030ee2a05af");
        var accountId = Guid.Parse("d011d532-2eef-433a-8fd6-d3424adb607f");
        return AccountCreator.CreateAccount(accountId, ownerId);
    }
}