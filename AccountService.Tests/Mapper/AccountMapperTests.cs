using AccountService.Features.Accounts;
using AccountService.Features.Accounts.Dto;
using AccountService.Features.Transactions;
using AccountService.Tests.Generator;
using AccountService.Tests.Utils;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AccountService.Tests.Mapper;

public class AccountMapperTests
{
    private readonly IMapper _mapper;

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
        var account = AccountGenerator.CreateAccount();

        //Act
        var destination = _mapper.Map<AccountResponseShortDto>(account);

        //Assert
        AccountAssert.AssertAccountAndShortDto(account, destination);
    }

    [Fact]
    public void MapToFullDto_Success()
    {
        //Arrange
        var account = AccountGenerator.CreateAccount();

        //Act
        var destination = _mapper.Map<AccountResponseFullDto>(account);

        //Assert
        AccountAssert.AssertAccountAndFullDto(account, destination);
    }

    [Fact]
    public void MapToTransactionEntityFromCreate_Success()
    {
        //Arrange
        var command = AccountGenerator.CreateCommand();

        //Act
        var destination = _mapper.Map<Account>(command);

        //Assert
        AccountAssert.AssertCreateCommandAndEntity(command, destination);
    }
}