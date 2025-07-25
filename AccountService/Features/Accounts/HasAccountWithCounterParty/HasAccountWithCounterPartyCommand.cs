using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace AccountService.Features.Accounts.HasAccountWithCounterParty;

[SwaggerSchema(Description = "Data transfer object for find account by id",
    Required = new[] { "accountId", "ownerId" })]
public class HasAccountWithCounterPartyCommand : IRequest<bool>
{
    public HasAccountWithCounterPartyCommand(Guid ownerId, Guid accountId)
    {
        OwnerId = ownerId;
        AccountId = accountId;
    }

    [SwaggerSchema("counterparty id")] public Guid OwnerId { get; }

    [SwaggerSchema("account id")] public Guid AccountId { get; }
}