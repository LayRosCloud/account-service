using AccountService.Utils.Extensions;
using AccountService.Utils.Middleware;
using FluentValidation;
using System.Reflection;
using AccountService.Broker.Client;
using AccountService.Broker.Events;
using AccountService.Features.Accounts;
using AccountService.Features.Transactions.DailyPercentAddToAccount;
using AccountService.Utils.Extensions.Configuration;
using Broker.Handlers;
using FluentMigrator.Runner;
using Hangfire;
using Hangfire.PostgreSql;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddControllers();
services.AddHttpContextAccessor();
services.AddCorsPolicy();
services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddFluentMigratorConsole();
});

services.AddScopedInjections();
if (!builder.Environment.IsEnvironment("Testing"))
{
    services.AddDbContextMigrations(builder.Configuration);
    services.AddHangfire(config =>
    {
    #pragma warning disable CS0618 // Type or member is obsolete
        config.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("Connect"));
    #pragma warning restore CS0618 // Type or member is obsolete
    });
    services.AddHangfireServer();
}
services.AddSwaggerGenAuthorization(builder.Configuration);
services.AddAuthorization();
services.SettingAuthorization(builder.Configuration);
services.AddMemoryCache();
services.AddLogging();
services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
services.AddMediatR(options =>
{
    options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    options.AddOpenBehavior(typeof(ValidatorBehaviour<,>));
});

services.AddApplicationProfiles();

var app = builder.Build();

app.UseMiddleware<CorrelationMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LoggerMiddleware>();
if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var service = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    service.MigrateUp();
    app.UseHangfireDashboard();
    var hangJob = scope.ServiceProvider.GetRequiredService<DailyPercentAddedToAccount>();
    RecurringJob.AddOrUpdate(
        "AccrueInterest",
        () => hangJob.AccrueInterest(),
        Cron.Daily);
    var connection = scope.ServiceProvider.GetRequiredService<IConnectionBroker>().Connection;
    var repository = scope.ServiceProvider.GetRequiredService<IEventRepository>();
    var accountRepository = scope.ServiceProvider.GetRequiredService<IAccountRepository>();
    await using var channel = await connection!.CreateChannelAsync();
    var consumer = await ClientConsumer.CreateAsync(channel, repository, accountRepository);
    await consumer.ConsumeAsync(CancellationToken.None);
}
app.UseCors(CorsConfigurationExtensions.CorsPolicy);
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    const string path = "/swagger-ui/swagger.css";
    options.InjectStylesheet(path);
});


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program {}