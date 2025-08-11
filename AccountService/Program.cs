using AccountService.Utils.Extensions;
using AccountService.Utils.Middleware;
using FluentValidation;
using System.Reflection;
using AccountService.Features.Transactions.DailyPercentAddToAccount;
using AccountService.Utils.Extensions.Configuration;
using FluentMigrator.Runner;
using Hangfire;
using Hangfire.PostgreSql;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddControllers();
services.AddHttpContextAccessor();
services.AddDbContextMigrations(builder.Configuration);

services.AddCorsPolicy();
services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddFluentMigratorConsole();
});

services.AddScopedInjections();
services.AddHangfire(config =>
{
#pragma warning disable CS0618 // Type or member is obsolete
    config.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("Connect"));
#pragma warning restore CS0618 // Type or member is obsolete
});
services.AddHangfireServer();
services.AddSwaggerGenAuthorization(builder.Configuration);
services.AddAuthorization();
services.SettingAuthorization(builder.Configuration);
services.AddMemoryCache();
services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
services.AddMediatR(options =>
{
    options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    options.AddOpenBehavior(typeof(ValidatorBehaviour<,>));
});

services.AddApplicationProfiles();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    if (service.HasMigrationsToApplyUp())
    {
        service.MigrateUp();
    }
}
app.UseCors(CorsConfigurationExtensions.CorsPolicy);
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    const string path = "/swagger-ui/swagger.css";
    options.InjectStylesheet(path);
});

app.UseHangfireDashboard();
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<DailyPercentAddedToAccount>();
    RecurringJob.AddOrUpdate(
        "daily-transactions",
        () => service.DailyPercentAsync(),
        Cron.Daily);
}


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();