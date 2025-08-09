namespace AccountService.Utils.Extensions.Configuration;

public static class CorsConfigurationExtensions
{
    public const string CorsPolicy = "LocalOrigins";

    public static IServiceCollection AddCorsPolicy(this IServiceCollection service)
    {
        service.AddCors(options =>
        {
            options.AddPolicy(CorsPolicy, policyOptions =>
            {
                policyOptions.SetIsOriginAllowed(domain => domain.StartsWith("http://localhost"))
                    .WithMethods("GET", "POST", "PATCH", "DELETE")
                    .WithHeaders("Authorization", "Content-Type", "Accept")
                    .AllowCredentials()
                    .SetPreflightMaxAge(TimeSpan.FromHours(1))
                    .Build();
            });
        });
        return service;
    }
}