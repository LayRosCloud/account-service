using System.ComponentModel.DataAnnotations;
using MediatR;

namespace AccountService.Features.Accounts.DeleteAccount;

public class DeleteAccountCommand : IRequest<Unit>
{
    public DeleteAccountCommand(Guid accountId)
    {
        AccountId = accountId;
    }

    /// <summary>
    /// account id
    /// </summary>
    [Required] 
    public Guid AccountId { get; }
}