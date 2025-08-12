using System.Net;
using System.Net.Http.Json;
using AccountService.Features.Transactions;
using AccountService.Features.Transactions.TransferBetweenAccounts;
using AccountService.Tests.Controllers.Utils;
using AccountService.Tests.Generator;
using AccountService.Utils.Data;
using Microsoft.Extensions.DependencyInjection;

namespace AccountService.Tests.Controllers;

public class TransactionControllerTests : IClassFixture<ContainerTests<DatabaseContext>>
{
    private readonly ContainerTests<DatabaseContext> _container;

    public TransactionControllerTests(ContainerTests<DatabaseContext> container)
    {
        _container = container;
    }

    [Fact]
    public async Task MakeTransferRequest_Success()
    {
        //Arrange
        var client = _container.CreateClient();
        const int count = 50;
        const int amount = 100;
        var context = _container.Services.GetRequiredService<DatabaseContext>();
        var accountFromId = Guid.Parse("78346cd0-7ec7-4b81-835c-ab660114c9b2");
        var accountToId = Guid.Parse("de00f990-1993-472d-9153-a60a8738c152");
        await context.Accounts.AddAsync(AccountCreator.CreateAccount(accountFromId, Guid.NewGuid(), balance: count * amount,
            createdAt: DateTimeOffset.UtcNow));
        await context.Accounts.AddAsync(AccountCreator.CreateAccount(accountToId, Guid.NewGuid(), balance: 0,
            createdAt: DateTimeOffset.UtcNow));
        await context.SaveChangesAsync();
        var jsonContent = JsonContent.Create(new TransferBetweenAccountsCommand()
        {
            AccountId = accountFromId,
            CounterPartyAccountId = accountToId,
            Description = "Transfer",
            Sum = amount,
            Type = TransactionType.Credit
        });

        //Act
        var result = await client.PostAsync("/transactions/transfer", jsonContent);

        // Assert
        Assert.Equal(HttpStatusCode.Created, result.StatusCode);
    }

    [Fact]
    public async Task Make50Requests_Success()
    {
        //Arrange
        var client = _container.CreateClient();
        const int count = 50;
        const int amount = 100;
        var semaphore = new SemaphoreSlim(0, count);
        var tasks = new Task<HttpResponseMessage>[count];
        var context = _container.Services.GetRequiredService<DatabaseContext>();
        var accountFromId = Guid.Parse("bd993442-b5c6-48b3-8475-540ff586260a");
        var accountToId = Guid.Parse("e4c14fc3-db48-49e3-a71b-98a0ca6958ea");
        await context.Accounts.AddAsync(AccountCreator.CreateAccount(accountFromId, Guid.NewGuid(), balance: count * amount,
            createdAt: DateTimeOffset.UtcNow));
        await context.Accounts.AddAsync(AccountCreator.CreateAccount(accountToId, Guid.NewGuid(), balance: 0,
            createdAt: DateTimeOffset.UtcNow));
        await context.SaveChangesAsync();
        for (var i = 0; i < count; i++)
        {
            var jsonContent = JsonContent.Create(new TransferBetweenAccountsCommand()
            {
                AccountId = accountFromId,
                CounterPartyAccountId = accountToId,
                Description = "Transfer",
                Sum = amount,
                Type = TransactionType.Credit
            });
            tasks[i] = GetTransferResult(semaphore, client, jsonContent);
        }
        //Act
        semaphore.Release(count);
        var result = await Task.WhenAll(tasks);

        // Assert
        Assert.Equal(1, result.Count(x => x.IsSuccessStatusCode));
        Assert.Equal(49, result.Count(x=>x.StatusCode == HttpStatusCode.Conflict));
    }

    private async Task<HttpResponseMessage> GetTransferResult(SemaphoreSlim semaphore, HttpClient client, HttpContent content)
    {
        await semaphore.WaitAsync();
        return await client.PostAsync("/transactions/transfer", content);
    }
}