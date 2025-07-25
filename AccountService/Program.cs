using System.Reflection;
using AccountService.Features.Accounts;
using AccountService.Features.Transactions;
using AccountService.Utils.Middleware;
using FluentValidation;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Account service", Version = "v1" });
    options.EnableAnnotations();
});
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    options.AddOpenBehavior(typeof(ValidatorBehaviour<,>));
});

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(typeof(AccountMapper));
    config.AddProfile(typeof(TransactionMapper));
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.StylesPath = "https://cdnjs.cloudflare.com/ajax/libs/swagger-ui/5.21.0/swagger-ui.min.css";
});

app.UseAuthorization();

app.MapControllers();

app.Run();