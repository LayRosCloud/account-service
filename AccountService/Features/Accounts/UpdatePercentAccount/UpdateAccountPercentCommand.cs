using AccountService.Features.Accounts.Dto;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace AccountService.Features.Accounts.UpdatePercentAccount;

[SwaggerSchema(Description = "Data transfer object for find account by id",
    Required = new[] { "id" })]
public class UpdateAccountPercentCommand : IRequest<AccountResponseShortDto>
{
    [SwaggerSchema("account id", ReadOnly = true)]
    public Guid Id { get; set; }

    [SwaggerSchema("percent for credit account")]
    public decimal? Percent { get; set; }
}