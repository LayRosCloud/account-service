using AccountService.Features.Accounts;
using AccountService.Features.Transactions;
using AccountService.Utils.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace AccountService.Tests.Features.AccountsTests;

public abstract class AccountTests
{
    protected Mock<IAccountRepository> AccountRepositoryMock { get; }
    protected Mock<ITransactionWrapper> TransactionWrapperMock { get; }
    protected IMapper Mapper { get; }
    protected Mock<IStorageContext> StorageMock { get; }

    // ReSharper disable once ConvertConstructorToMemberInitializers
    protected AccountTests()
    {
        AccountRepositoryMock = new();
        TransactionWrapperMock = new();
        StorageMock = new();
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddAutoMapper(x => x.AddProfile(typeof(AccountMapper)));
        serviceCollection.AddAutoMapper(x => x.AddProfile(typeof(TransactionMapper)));

        var serviceProvider = serviceCollection.BuildServiceProvider();
        Mapper = serviceProvider.GetRequiredService<IMapper>();
    }

    protected static void SetupTransaction<TResult>(Mock<ITransactionWrapper> transactionMock)
    {
        transactionMock
            .Setup(w => w.Execute(
                It.IsAny<Func<IDbContextTransaction?, Task<TResult>>>(),
                It.IsAny<CancellationToken>()))
            .Returns<Func<IDbContextTransaction?, Task<TResult>>, CancellationToken>(
                (func, _) => func(null));
    }
}