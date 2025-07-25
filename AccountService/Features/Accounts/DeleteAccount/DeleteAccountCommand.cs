using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace AccountService.Features.Accounts.DeleteAccount;

[SwaggerSchema(Description = "Data transfer object for delete account",
    Required = new[] { "accountId" })]
public class DeleteAccountCommand : IRequest<int>
{
    [SwaggerSchema("id account")]
    public Guid AccountId { get; }

    public DeleteAccountCommand(Guid accountId)
    {
        AccountId = accountId;
    }
}