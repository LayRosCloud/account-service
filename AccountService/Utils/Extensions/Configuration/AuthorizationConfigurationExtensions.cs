using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AccountService.Utils.Extensions.Configuration;

public static class AuthorizationConfigurationExtensions
{
    public static IServiceCollection SettingAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.Audience = configuration["Authorization:Audience"];
                options.MetadataAddress = configuration["Authorization:MetadataAddress"]!;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["Authorization:ValidIssuer"]
                };
            });
        return services;
    }
}