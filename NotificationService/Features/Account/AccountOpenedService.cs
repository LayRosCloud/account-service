using Broker.AccountService;
using NotificationService.Features.Broker;

namespace NotificationService.Features.Account;

public class AccountOpenedService
{
    public static readonly Dictionary<string, Func<AccountOpenedEvent, IEventRepository, Task>> Actions = new()
    {
        {"v1", SendAsync }
    };

    public static async Task SendAsync(AccountOpenedEvent accountOpenedEvent, IEventRepository repository)
    {
        if (await repository.ExistsEventAsync(accountOpenedEvent.EventId))
        {
            return;
        }
        await repository.CreateAsync(new EventEntity(accountOpenedEvent.EventId, DateTime.UtcNow, "Account.events"));
        
        Console.WriteLine("Handle event repository");
    }
}