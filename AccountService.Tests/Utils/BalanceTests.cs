namespace AccountService.Tests.Utils;

public abstract class BalanceTests
{
    protected Guid OwnerId { get; } = Guid.Parse("d011d532-2eef-433a-8fd6-d3424adb607f");
    protected Guid AccountId { get; } = Guid.Parse("defd9e37-b69d-4813-8bf7-6030ee2a05af");
    protected DateTimeOffset CreatedAt { get; } = new(2020, 10, 20, 10, 22, 22, new TimeSpan(3, 0, 0));
}