using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace AccountService.Tests.Controllers.Utils;

public class IntegrationTests : IClassFixture<ApplicationFactory>, IAsyncLifetime
{
    public IntegrationTests(ApplicationFactory factory)
    {
        Factory = factory;
        Container = new ContainerBuilder()
            .WithImage("mcr.microsoft.com/dotnet/sdk:9.0")
            .WithPortBinding(8080, assignRandomHostPort: true)
            .WithCommand("dotnet", "run", "--urls", "http://*:8080")
            .Build();
    }

    protected ApplicationFactory Factory { get; }
    protected IContainer Container { get; }

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await Container.StopAsync();
    }
}