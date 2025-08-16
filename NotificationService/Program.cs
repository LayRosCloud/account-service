using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Data;
using NotificationService.Data.Migrations;
using NotificationService.Features.Broker;

var services = new ServiceCollection();
services.AddScoped<IStorageContext, DatabaseContext>();
services.AddScoped<IEventRepository, EventRepository>();
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

var runner = provider.GetRequiredService<IMigrationRunner>();
runner.MigrateUp();

Console.ReadLine();