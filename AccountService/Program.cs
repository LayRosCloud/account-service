using AccountService.Utils.Extensions;
using AccountService.Utils.Middleware;
using FluentValidation;
using System.Reflection;
using AccountService.Utils.Extensions.Configuration;
using FluentMigrator.Runner;

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();