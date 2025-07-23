using AccountService.Features.Accounts;
using AccountService.Utils.Middleware;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehaviour<,>));
builder.Services.AddMediatR(config =>
{
    config.AddOpenBehavior(typeof(ValidatorBehaviour<,>));
});

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(typeof(AccountMapper));
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
