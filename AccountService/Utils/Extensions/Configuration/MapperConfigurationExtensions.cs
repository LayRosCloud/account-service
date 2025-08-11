using AccountService.Features.Accounts;
using AccountService.Features.Transactions;

namespace AccountService.Utils.Extensions.Configuration;

public static class MapperConfigurationExtensions
{
    public static void AddApplicationProfiles(this IServiceCollection service)
    {
        service.AddAutoMapper(configuration =>
        {
            configuration.AddProfile<AccountMapper>();
            configuration.AddProfile<TransactionMapper>();
        });
        
    }
}