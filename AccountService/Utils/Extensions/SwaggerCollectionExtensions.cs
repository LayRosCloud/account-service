using Microsoft.OpenApi.Models;
using System.Reflection;

namespace AccountService.Utils.Extensions;

public static class SwaggerCollectionExtensions
{
    private const string XmlFormat = "xml";
    private const string VersionApi = "v1";
    private const string NameApi = "Account service";
    private const string NameSecurityDefinition = "KeyCloak";
    private const string TokenType = "Bearer";
    private const string ConfigurationAuthorizationUrl = "KeyCloak:AuthorizationUrl";
    private const string ScopeOpenId = "openid";
    private const string ScopeProfile = "profile";

    public static IServiceCollection AddSwaggerGenAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(opt =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.{XmlFormat}";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            opt.IncludeXmlComments(xmlPath);
            opt.SwaggerDoc(VersionApi, new OpenApiInfo { Title = NameApi, Version = VersionApi });

            opt.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));

            opt.AddSecurityDefinition(NameSecurityDefinition, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(configuration[ConfigurationAuthorizationUrl]!),
                        Scopes = new Dictionary<string, string>()
                        {
                            { ScopeOpenId, ScopeOpenId },
                            { ScopeProfile, ScopeProfile }
                        }
                    }
                }
            });

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = NameSecurityDefinition,
                            Type = ReferenceType.SecurityScheme
                        },
                        In = ParameterLocation.Header,
                        Name = TokenType,
                        Scheme = TokenType
                    },
                    new List<string>()
                }
            };

            opt.AddSecurityRequirement(securityRequirement);
        });
        return services;
    }
}