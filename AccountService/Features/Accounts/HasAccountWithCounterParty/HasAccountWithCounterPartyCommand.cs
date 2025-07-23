using MediatR;

namespace AccountService.Features.Accounts.HasAccountWithCounterParty
{
    public class HasAccountWithCounterPartyCommand : IRequest<bool>
    {
        public HasAccountWithCounterPartyCommand(Guid ownerId, Guid accountId)
        {
            OwnerId = ownerId;
            AccountId = accountId;
        }

        public Guid OwnerId { get; }
        public Guid AccountId { get; }
    }
}
