using AccountService.Utils.Data.Migrations;
using Broker.Entity;
using Broker.Handlers;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Testcontainers.PostgreSql;

namespace AccountService.Tests.Controllers.Utils;

public class ContainerTests<TDbContext> : IAsyncLifetime, IDisposable where TDbContext : DbContext
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithUsername("admin")
        .WithPassword("admin")
        .WithDatabase("account_base")
        .WithImage("postgres:16.2")
        .WithPortBinding(0, 5432)
        .WithCleanUp(true)
        .Build();

    private WebApplicationFactory<Program>? _factory;
    private IServiceScope? _scope;


    public IServiceProvider Services => _factory?.Services!;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<TDbContext>));


                if (descriptor != null)
                    services.Remove(descriptor);

                var descriptorMigrate = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IMigrationRunner));
                if (descriptorMigrate != null)
                    services.Remove(descriptorMigrate);

                var descriptorRabbitMq = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IMessageProducer));
                if (descriptorRabbitMq != null)
                    services.Remove(descriptorRabbitMq);
                var producerMock = new Mock<IMessageProducer>();
                producerMock.Setup(x =>
                    x.SendAsync(It.IsAny<EventWrapper>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()
                    )).Returns(Task.CompletedTask);
                services.AddSingleton(producerMock.Object);
                services.AddDbContext<TDbContext>(options =>
                    options.UseNpgsql(_dbContainer.GetConnectionString())
                        .EnableDetailedErrors()
                        .EnableSensitiveDataLogging());

                services.AddFluentMigratorCore()
                    .ConfigureRunner(runner =>
                        runner.AddPostgres()
                            .WithGlobalConnectionString(_dbContainer.GetConnectionString())
                            .ScanIn(typeof(InitializeSchema).Assembly).For.Migrations());

                services.AddAuthentication("TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", _ => { });
            });
        });
        _scope = _factory.Services.CreateScope();
        // ReSharper disable once IdentifierTypo
        var migrator = _scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        migrator.MigrateUp();
    }

    public async Task DisposeAsync()
    {
        _scope?.Dispose();
        await _factory!.DisposeAsync();
        await _dbContainer.DisposeAsync();
    }

    public HttpClient CreateClient()
    {
        return _factory!.CreateClient();
    }

    public void Dispose()
    {
        _dbContainer.DisposeAsync().AsTask();
        _factory!.Dispose();
        _scope!.Dispose();
    }
}