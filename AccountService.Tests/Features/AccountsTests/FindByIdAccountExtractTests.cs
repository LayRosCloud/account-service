using AccountService.Features.Accounts;
using AccountService.Features.Accounts.FindByIdAccountExtract;
using AccountService.Features.Transactions;
using AccountService.Tests.Generator;
using Moq;

namespace AccountService.Tests.Features.AccountsTests;

public class FindByIdAccountExtractTests : AccountTests
{
    [Fact]
    public async Task FindByIdAccountsExtract_Success()
    {
        //Arrange
        var id = Guid.Parse("d5aa9996-7fe9-4584-b23c-4a1899f81e04");
        var account = AccountCreator.CreateAccount(id, Guid.NewGuid());
        var account2 = AccountCreator.CreateAccount(Guid.NewGuid(), Guid.NewGuid());
        var list1 = new List<Transaction>(5);
        var list2 = new List<Transaction>(5);
        var date = new DateTimeOffset(2022, 1, 1, 12, 23, 32, 44, 21, new TimeSpan(3, 0, 0));
        var dateStart = date.AddHours(-1);
        var dateEnd = date.AddHours(1);
        for (var i = 0; i < 5; i++) 
            list1.Add(TransactionCreator.CreateTransaction(Guid.NewGuid(), account, createdAt: date));

        for (var i = 0; i < 5; i++)
            list2.Add(TransactionCreator.CreateTransaction(Guid.NewGuid(), account2, account, createdAt:date));
        list1[0].CreatedAt = DateTimeOffset.MinValue;
        list1[1].CreatedAt = DateTimeOffset.MinValue;

        account.AccountTransactions.AddRange(list1);
        account.CounterPartyTransactions.AddRange(list2);

        AccountRepositoryMock.Setup(x => x.FindByIdAsync(id, true))
            .ReturnsAsync(account);

        var command = new FindByIdAccountExtractQuery(id, dateStart, dateEnd);
        var handler = new FindByIdAccountExtractHandler(Mapper, AccountRepositoryMock.Object);

        SetupTransaction<Account>(TransactionWrapperMock);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.Equal(3, result.Transactions.Count);
    }

    [Fact]
    public async Task FindByIdAccountsExtract_EqualsDate_Success()
    {
        //Arrange
        var id = Guid.Parse("d5aa9996-7fe9-4584-b23c-4a1899f81e04");
        var account = AccountCreator.CreateAccount(id, Guid.NewGuid());
        var account2 = AccountCreator.CreateAccount(Guid.NewGuid(), Guid.NewGuid());
        var list1 = new List<Transaction>(5);
        var list2 = new List<Transaction>(5);
        var date = new DateTimeOffset(2022, 1, 1, 12, 23, 32, 44, 21, new TimeSpan(3, 0, 0));
        for (var i = 0; i < 5; i++)
            list1.Add(TransactionCreator.CreateTransaction(Guid.NewGuid(), account, createdAt: date));

        for (var i = 0; i < 5; i++)
            list2.Add(TransactionCreator.CreateTransaction(Guid.NewGuid(), account2, account, createdAt: date));
        list1[0].CreatedAt = DateTimeOffset.MinValue;
        list1[1].CreatedAt = DateTimeOffset.MinValue;

        account.AccountTransactions.AddRange(list1);
        account.CounterPartyTransactions.AddRange(list2);

        AccountRepositoryMock.Setup(x => x.FindByIdAsync(id, true))
            .ReturnsAsync(account);

        var command = new FindByIdAccountExtractQuery(id, date, date);
        var handler = new FindByIdAccountExtractHandler(Mapper, AccountRepositoryMock.Object);

        SetupTransaction<Account>(TransactionWrapperMock);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.Equal(3, result.Transactions.Count);
    }
}