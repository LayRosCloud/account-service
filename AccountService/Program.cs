using AccountService.Features.Accounts;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddMediatR( _ =>
{
    
});
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(typeof(AccountMapper));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
