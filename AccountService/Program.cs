using System.Reflection;
using AccountService.Features.Accounts;
using AccountService.Features.Transactions;
using AccountService.Utils.Data;
using AccountService.Utils.Middleware;
using FluentValidation;
using Microsoft.OpenApi.Models;

const string corsPolicy = "LocalOrigins";

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddControllers();
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

services.AddSingleton<IDatabaseContext, MemoryContext>();
services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Account service", Version = "v1" });
});

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
    options.InjectStylesheet("swagger.css");
});

app.UseAuthorization();

app.MapControllers();

app.Run();