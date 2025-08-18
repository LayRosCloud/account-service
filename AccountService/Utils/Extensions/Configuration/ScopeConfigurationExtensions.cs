using AccountService.Broker;
using AccountService.Broker.Account;
using AccountService.Broker.Events;
using AccountService.Broker.Transaction;
using AccountService.Features.Accounts;
using AccountService.Features.Transactions;
using AccountService.Features.Transactions.DailyPercentAddToAccount;
using AccountService.Features.Transactions.Utils.Transfer;
using AccountService.Features.Users.Utils;
using AccountService.Utils.Data;
using Broker.AccountService;
using Broker.Handlers;

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
        services.AddSingleton<IConnectionBroker, RabbitMqConnection>();
        services.AddScoped<IMessageProducer, RabbitMqProducer>();
        services.AddScoped<IProducer<AccountOpenedEvent>, AccountProducer>();
        services.AddScoped<ITransactionProducer, TransactionProducer>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IProducer<TransferCompletedEvent>, TransactionProducer>();
        services.AddScoped<IProducer<InterestAccruedEvent>, InterestAccruedProducer>();

        return services;
    }
}