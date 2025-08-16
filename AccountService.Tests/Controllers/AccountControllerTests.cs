using AccountService.Features.Accounts;
using AccountService.Features.Accounts.Dto;
using AccountService.Features.Accounts.UpdatePercentAccount;
using AccountService.Tests.Asserts;
using AccountService.Tests.Controllers.Utils;
using AccountService.Tests.Generator;
using AccountService.Utils.Data;
using AccountService.Utils.Result;
using System.Net;
using System.Net.Http.Json;
using Broker.AccountService;
using Microsoft.Extensions.DependencyInjection;

namespace AccountService.Tests.Controllers;

public class AccountControllerTests : IClassFixture<ContainerTests<DatabaseContext>>, IAsyncDisposable
{
    private readonly ContainerTests<DatabaseContext> _container;

    public AccountControllerTests(ContainerTests<DatabaseContext> container)
    {
        _container = container;
    }

    [Fact]
    public async Task FindAllAccounts_Success()
    {
        //Arrange
        using var client = _container.CreateClient();
        var ctx = _container.Services.GetRequiredService<DatabaseContext>();
        const int expected = 10;
        var list = new List<Account>(expected);
        for (int i = 0; i < expected; i++)
        {
            list.Add(AccountCreator.CreateAccount(Guid.NewGuid(), Guid.NewGuid(), createdAt: DateTimeOffset.UtcNow));
        }

        await ctx.Accounts.AddRangeAsync(list);
        await ctx.SaveChangesAsync();

        //Act
        var result = await client.GetFromJsonAsync<MbResponse<List<AccountResponseShortDto>>>("/accounts");

        //Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task FindById_Success()
    {
        //Arrange
        using var client = _container.CreateClient();
        var ctx = _container.Services.GetRequiredService<DatabaseContext>();
        var accountId = Guid.Parse("c4754a55-9e0c-41c2-bae8-e83f0dcec862");
        var account = AccountCreator.CreateAccount(accountId, Guid.NewGuid(), createdAt: DateTimeOffset.UtcNow);
        var resultFromBase = await ctx.Accounts.AddAsync(account);
        await ctx.SaveChangesAsync();
        //Act
        var result = await client.GetFromJsonAsync<MbResponse<AccountResponseFullDto>>($"/accounts/{accountId}");

        //Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        Assert.Equal(200, result.StatusCode);
        AccountAssert.Equal(resultFromBase.Entity, result.Result);
    }

    [Fact]
    public async Task FindById_NotFound()
    {
        //Arrange
        using var client = _container.CreateClient();

        //Act
        var response = await client.GetAsync($"/accounts/{new Guid()}");

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdatePercent_Ok()
    {
        //Arrange
        using var client = _container.CreateClient();
        var ctx = _container.Services.GetRequiredService<DatabaseContext>();
        var accountId = Guid.Parse("356b4fee-2bed-4219-8e41-d2771b5d0585");
        var account = AccountCreator.CreateAccount(accountId, Guid.NewGuid(), createdAt: DateTimeOffset.UtcNow);
        var entry = await ctx.Accounts.AddAsync(account);
        var entity = entry.Entity;
        await ctx.SaveChangesAsync();
        var content = JsonContent.Create(new { percent = 1 });
        //Act
        var result = await client.PatchAsync($"/accounts/{entity.Id}/percent", content);

        //Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async Task UpdatePercent_NotFound()
    {
        //Arrange
        using var client = _container.CreateClient();
        var content = JsonContent.Create(new { percent = 1 });
        //Act
        var result = await client.PatchAsync($"/accounts/{Guid.Parse("a3acd351-ef87-4948-8e80-706f41b357ef")}/percent", content);

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact]
    public async Task UpdatePercent_CreditTypePercentNull_ValidationFailure()
    {
        //Arrange
        using var client = _container.CreateClient();
        var ctx = _container.Services.GetRequiredService<DatabaseContext>();
        var accountId = Guid.Parse("34186408-9f65-4210-9f14-93d3c2bf8a76");
        var account = AccountCreator.CreateAccount(accountId, Guid.NewGuid(), createdAt: DateTimeOffset.UtcNow, type: AccountType.Credit);
        await ctx.Accounts.AddAsync(account);
        await ctx.SaveChangesAsync();
        var content = JsonContent.Create(new UpdateAccountPercentCommand() { Id = accountId, Percent = null });
        //Act
        var result = await client.PatchAsync($"/accounts/{accountId}/percent", content);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }

    [Fact]
    public async Task UpdateType_Ok()
    {
        //Arrange
        using var client = _container.CreateClient();
        var ctx = _container.Services.GetRequiredService<DatabaseContext>();
        var accountId = Guid.Parse("8262ab49-3049-49d8-a2e7-a4cff75ef4c3");
        var account = AccountCreator.CreateAccount(accountId, Guid.NewGuid(), createdAt: DateTimeOffset.UtcNow, type: AccountType.Credit);
        var entry = await ctx.Accounts.AddAsync(account);
        var entity = entry.Entity;
        await ctx.SaveChangesAsync();
        var content = JsonContent.Create(new { type = AccountType.Checking });
        //Act
        var result = await client.PatchAsync($"/accounts/{entity.Id}/type", content);

        //Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }


    public async ValueTask DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}