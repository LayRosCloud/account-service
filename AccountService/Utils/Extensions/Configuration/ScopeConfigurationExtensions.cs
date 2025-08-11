using AccountService.Features.Accounts;
using AccountService.Features.Transactions;
using AccountService.Features.Transactions.DailyPercentAddToAccount;
using AccountService.Features.Transactions.Utils.Transfer;
using AccountService.Features.Users.Utils;
using AccountService.Utils.Data;

namespace AccountService.Utils.Extensions.Configuration;

public static class ScopeConfigurationExtensions
{
    public static IServiceCollection AddScopedInjections(this IServiceCollection services)
    {
        services.AddScoped<ITransactionWrapper, TransactionWrapper>();
        services.AddScoped<ITransferFactory, TransferFactory>();
        services.AddScoped<IStorageContext, DatabaseContext>();
        services.AddScoped<IKeyCloakClient, KeyCloakClient>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<DailyPercentAddedToAccount>();

        return services;
    }
}