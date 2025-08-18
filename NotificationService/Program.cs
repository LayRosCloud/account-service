using Broker.Handlers;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Data;
using NotificationService.Data.Migrations;
using NotificationService.Extensions;
using NotificationService.Features.Account;
using NotificationService.Features.Broker;
using NotificationService.Features.Transaction;

var services = new ServiceCollection();
services.AddScoped<IStorageContext, DatabaseContext>();
services.AddScoped<IEventRepository, EventRepository>();
services.AddScoped<IConnectionBroker, RabbitMqConnection>();
services.AddScoped<IMessageProducer, RabbitMqProducer>();
services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql("Host=postgres_notification;Port=5432;Database=event_base;Username=admin;Password=admin");
});

services.AddFluentMigratorCore()
    .ConfigureRunner(runner =>
        runner.AddPostgres()
            .WithGlobalConnectionString("Host=postgres_notification;Port=5432;Database=event_base;Username=admin;Password=admin")
            .ScanIn(typeof(InitializeSchema).Assembly).For.Migrations()
    );

var provider = services.BuildServiceProvider();

var migrationRunner = provider.GetRequiredService<IMigrationRunner>();
migrationRunner.MigrateUp();
var repository = provider.GetRequiredService<IEventRepository>();

var connection = provider.GetRequiredService<IConnectionBroker>().Connection!;
await provider.ListenAsync(connection, async (channel) =>
    await AccountConsumer.CreateAsync(repository, channel));
var consumer = await TransactionMessageConsumer.CreateAsync(repository, connection);
await consumer.ConsumeAsync(CancellationToken.None);



Console.ReadLine();
