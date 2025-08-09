using AccountService.Utils.Data;
using AccountService.Utils.Data.Migrations;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Utils.Extensions.Configuration;

public static class DatabaseConfigurationExtensions
{
    public static IServiceCollection AddDbContextMigrations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Connect"));
        });

        services.AddFluentMigratorCore()
            .ConfigureRunner(runner =>
                runner.AddPostgres()
                    .WithGlobalConnectionString("Connect")
                    .ScanIn(typeof(InitializeSchema).Assembly)
            );
        return services;
    }
}