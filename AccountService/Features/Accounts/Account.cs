using System.ComponentModel.DataAnnotations;
using AccountService.Features.Transactions;

namespace AccountService.Features.Accounts;

public class Account
{
    private decimal _balance;
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public AccountType Type { get; set; }
    public string Currency { get; set; } = string.Empty;

    public decimal Balance
    {
        get => _balance;
        set
        {
            if (Type == AccountType.Deposit && value < 0)
            {
                throw new ValidationException("The deposit account cannot be less than 0");
            }
            
            _balance = value;
        }
    }

    public decimal? Percent { get; set; }
    public long CreatedAt { get; set; }
    public long? ClosedAt { get; set; }
    public HashSet<Transaction> Transactions { get; } = new();
}