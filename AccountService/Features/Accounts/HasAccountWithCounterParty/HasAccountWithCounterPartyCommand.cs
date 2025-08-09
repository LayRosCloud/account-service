using System.ComponentModel.DataAnnotations;
using MediatR;

namespace AccountService.Features.Accounts.HasAccountWithCounterParty;

public class HasAccountWithCounterPartyCommand : IRequest<bool>
{
    public HasAccountWithCounterPartyCommand(Guid ownerId, Guid accountId)
    {
        OwnerId = ownerId;
        AccountId = accountId;
    }

    /// <summary>
    /// Owner id
    /// </summary>
    [Required] 
    public Guid OwnerId { get; }

    /// <summary>
    /// Account id
    /// </summary>
    [Required] 
    public Guid AccountId { get; }
}