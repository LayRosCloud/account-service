using AccountService.Features.Transactions;
using AccountService.Features.Transactions.Dto;
using AccountService.Tests.Generator;
using AccountService.Tests.Utils;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AccountService.Tests.Mapper;

public class TransactionMapperTests
{
    private readonly IMapper _mapper;

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
        var account = AccountGenerator.CreateAccount();
        var transaction = TransactionGenerator.CreateTransaction(account);

        //Act
        var destination = _mapper.Map<TransactionFullDto>(transaction);

        //Assert
        TransactionAssert.AssertTransactions(transaction, destination);
    }

    [Fact]
    public void MapToEntityFromCreateCommand_Success()
    {
        //Arrange
        var account = AccountGenerator.CreateAccount();
        var transaction = TransactionGenerator.CreateCommand(account);

        //Act
        var destination = _mapper.Map<Transaction>(transaction);

        //Assert
        TransactionAssert.AssertTransactions(destination, transaction);
    }

    [Fact]
    public void MapToTransactionFromTransferObject_Success()
    {
        //Arrange
        var accountFrom = AccountGenerator.CreateAccount();
        var accountTo = AccountGenerator.CreateAccount();
        var transaction = TransactionGenerator.CreateTransfer(accountFrom, accountTo);

        //Act
        var destination = _mapper.Map<Transaction>(transaction);

        //Assert
        TransactionAssert.AssertTransactions(destination, transaction);
    }

    [Fact]
    public void MapToTransactionFullDto_Success()
    {
        //Arrange
        var account = AccountGenerator.CreateAccount();
        var transaction = TransactionGenerator.CreateFullTransaction(account);

        //Act
        var destination = _mapper.Map<Transaction>(transaction);

        //Assert
        TransactionAssert.AssertTransactions(destination, transaction);
    }
}