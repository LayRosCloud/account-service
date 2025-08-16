using System.Text.Json.Serialization;
using AccountService.Features.Accounts.Dto;
using Broker.AccountService;
using MediatR;

namespace AccountService.Features.Accounts.UpdateTypeAccount;

public class UpdateTypeAccountCommand : IRequest<AccountResponseShortDto>
{
    public UpdateTypeAccountCommand(AccountType type)
    {
        Type = type;
    }

    /// <summary>
    /// Account id
    /// </summary>
    [JsonIgnore]
    public Guid AccountId { get; set; }

    /// <summary>
    /// Type of Account
    /// </summary>
    public AccountType Type { get; }
}