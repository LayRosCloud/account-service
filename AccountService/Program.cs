using AccountService.Features.Accounts;
using AccountService.Features.Transactions;
using AccountService.Features.Transactions.Utils.Transfer;
using AccountService.Features.Users.Utils;
using AccountService.Utils.Data;
using AccountService.Utils.Extensions;
using AccountService.Utils.Middleware;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

const string corsPolicy = "LocalOrigins";
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddControllers();
services.AddHttpContextAccessor();
services.AddTransient<ITransferFactory, TransferFactory>();

services.AddCors(options => 
    options.AddPolicy(corsPolicy, policyOptions =>
    {
        policyOptions.SetIsOriginAllowed(domain => domain.StartsWith("http://localhost"))
            .WithMethods("GET", "POST", "PATCH", "DELETE")
            .WithHeaders("Authorization", "Content-Type", "Accept")
            .AllowCredentials()
            .SetPreflightMaxAge(TimeSpan.FromHours(1))
            .Build();
    })
);
services.AddLogging(loggingBuilder => {
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});
services.AddSingleton<IDatabaseContext, MemoryContext>();
services.AddScoped<IKeyCloakClient, KeyCloakClient>();
services.AddSwaggerGenAuthorization(builder.Configuration);
services.AddAuthorization();
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.Audience = builder.Configuration["Authorization:Audience"];
        options.MetadataAddress = builder.Configuration["Authorization:MetadataAddress"]!;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Authorization:ValidIssuer"]
        };
    });
services.AddMemoryCache();
services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
services.AddMediatR(options =>
{
    options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    options.AddOpenBehavior(typeof(ValidatorBehaviour<,>));
});

services.AddAutoMapper(config =>
{
    config.AddProfile<AccountMapper>();
    config.AddProfile<TransactionMapper>();
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(corsPolicy);
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