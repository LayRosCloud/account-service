using AccountService.Features.Accounts;
using AccountService.Features.Accounts.Dto;
using AccountService.Features.Transactions;
using AccountService.Tests.Asserts;
using AccountService.Tests.Generator;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AccountService.Tests.Mapper;

public class AccountMapperTests
{
    private readonly IMapper _mapper;
    private readonly Guid _ownerId = Guid.Parse("d011d532-2eef-433a-8fd6-d3424adb607f");
    private readonly Guid _accountId = Guid.Parse("defd9e37-b69d-4813-8bf7-6030ee2a05af");

    public AccountMapperTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddAutoMapper(x => x.AddProfile(typeof(AccountMapper)));
        serviceCollection.AddAutoMapper(x => x.AddProfile(typeof(TransactionMapper)));

        var serviceProvider = serviceCollection.BuildServiceProvider();
        _mapper = serviceProvider.GetRequiredService<IMapper>();
    }

    [Fact]
    public void MapToShortDto_Success()
    {
        //Arrange
        var account = AccountCreator.CreateAccount(_accountId, _ownerId);

        //Act
        var destination = _mapper.Map<AccountResponseShortDto>(account);

        //Assert
        AccountAssert.Equal(account, destination);
    }

    [Fact]
    public void MapToFullDto_Success()
    {
        //Arrange
        var transactionIds = new[]
        {
            Guid.Parse("03059486-cfcb-42ba-96a7-9a8844bb6984"),
            Guid.Parse("a1fba02c-a263-4353-9102-e543914f58c3"),
            Guid.Parse("ba2a5373-2159-4334-b9fb-aef713fda7e4"),
            Guid.Parse("40536c3e-e76f-4bfc-8edc-1e0bb1f39507"),
            Guid.Parse("1c976a73-72b0-416a-9750-c7a21530ceab"),
        };
        var account = AccountCreator.CreateAccountWithTransaction(_accountId, _ownerId, transactionIds);

        //Act
        var destination = _mapper.Map<AccountResponseFullDto>(account);

        //Assert
        AccountAssert.Equal(account, destination);
    }

    [Fact]
    public void MapToTransactionEntityFromCreate_Success()
    {
        //Arrange
        var command = AccountCreator.CreateCommand(_ownerId);

        //Act
        var destination = _mapper.Map<Account>(command);

        //Assert
        AccountAssert.Equal(command, destination);
    }
}