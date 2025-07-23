using MediatR;

namespace AccountService.Features.Accounts.HasAccountWithCounterParty
{
    public class HasAccountWithCounterPartyCommand : IRequest<bool>
    {
        public HasAccountWithCounterPartyCommand(Guid counterPartyId, Guid accountId)
        {
            CounterPartyId = counterPartyId;
            AccountId = accountId;
        }

        public Guid CounterPartyId { get; }
        public Guid AccountId { get; }
    }
}
