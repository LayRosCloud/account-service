using AccountService.Features.Transactions.Dto;
using Broker.AccountService;

namespace AccountService.Features.Accounts.Dto;

public class AccountResponseFullDto
{
    /// <summary>
    /// Account id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Owner id
    /// </summary>
    public Guid OwnerId { get; set; }

    /// <summary>
    /// Type of account checking, credit, debit
    /// </summary>
    public AccountType Type { get; set; }

    /// <summary>
    /// Currency of account RUB, EUR and others
    /// </summary>
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// Balance of account
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// Percent of account
    /// </summary>
    public decimal? Percent { get; set; }

    /// <summary>
    /// Created date account
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Closed date account
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public DateTimeOffset? ClosedAt { get; set; }

    /// <summary>
    /// All transactions of accounts
    /// </summary>
    public List<TransactionFullDto> Transactions { get; set; } = new();
}