using AccountService.Features.Accounts;
using AccountService.Features.Accounts.Transactions;

namespace AccountService.Utils.Data;

public class DatabaseContext
{
    static DatabaseContext()
    {
        Instance = new DatabaseContext();
    }

    public static DatabaseContext Instance { get; }

    public List<Account> Accounts { get; } = new();
    public List<Transaction> Transactions { get; } = new();

    public List<Guid> CounterParties { get; } = new()
    {
        Guid.Parse("278857fc-746b-42ec-96fd-1e1bea494f69"),
        Guid.Parse("5b2b38c1-d86a-451d-8ceb-24b2c8f91d1f"),
        Guid.Parse("37fdadde-6ef8-4a46-99b6-9630da39f6c7"),
        Guid.Parse("e0bbb4b6-f1ef-4419-abd0-3206a0b75507"),
        Guid.Parse("8a536f1a-6314-481a-8f56-7d26321dbf2c")
    };
}