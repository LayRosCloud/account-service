using System.Text;
using System.Text.Json;
using Broker.AccountService;
using Broker.Handlers;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Data;
using NotificationService.Data.Migrations;
using NotificationService.Features.Account;
using NotificationService.Features.Broker;
using RabbitMQ.Client.Events;

var services = new ServiceCollection();
services.AddScoped<IStorageContext, DatabaseContext>();
services.AddScoped<IEventRepository, EventRepository>();
services.AddScoped<IConnectionBroker, RabbitMqConnection>();
services.AddScoped<IMessageProducer, RabbitMqProducer>();
services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql("Host=localhost;Port=5433;Database=event_base;Username=admin;Password=admin");
});

services.AddFluentMigratorCore()
    .ConfigureRunner(runner =>
        runner.AddPostgres()
            .WithGlobalConnectionString("Host=localhost;Port=5433;Database=event_base;Username=admin;Password=admin")
            .ScanIn(typeof(InitializeSchema).Assembly).For.Migrations()
    );

var provider = services.BuildServiceProvider();

var migrationRunner = provider.GetRequiredService<IMigrationRunner>();
migrationRunner.MigrateUp();
var connection = provider.GetRequiredService<IConnectionBroker>().Connection;
var channel = await connection!.CreateChannelAsync();

await channel.QueueDeclareAsync("account.events", false, false, false);

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (sender, eventArgs) =>
{
    var repository = provider.GetRequiredService<IEventRepository>();
    var bodyBytes = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(bodyBytes);
    var account = JsonSerializer.Deserialize<AccountOpenedEvent>(message);
    await ((AsyncEventingBasicConsumer)sender).Channel.BasicAckAsync(eventArgs.DeliveryTag, false);
    await AccountOpenedService.Actions[account!.Meta.Version].Invoke(account, repository);
};

Console.ReadLine();
