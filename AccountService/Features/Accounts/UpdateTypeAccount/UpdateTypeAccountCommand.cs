using AccountService.Features.Accounts.Dto;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace AccountService.Features.Accounts.UpdateTypeAccount;

[SwaggerSchema(Description = "Data transfer object for find account by id",
    Required = new[] { "type" })]
public class UpdateTypeAccountCommand : IRequest<AccountResponseShortDto>
{
    public UpdateTypeAccountCommand(AccountType type)
    {
        Type = type;
    }

    [SwaggerSchema("account id", ReadOnly = true)]
    public Guid AccountId { get; set; }

    [SwaggerSchema("type")] public AccountType Type { get; }
}