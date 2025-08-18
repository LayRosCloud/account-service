using AccountService.Features.Accounts;
using AccountService.Utils.Time;
using Broker.AccountService;
using FluentValidation;

namespace AccountService.Features.Transactions.Utils.Balance;

public class CreditBalance : IBalance
{
    private readonly TransactionType _type;

    public CreditBalance(Account account, TransactionType type)
    {
        if (account.Type != AccountType.Credit)
            throw new ValidationException("Error! Invalid type for credit balance");
        Account = account;
        _type = type;
    }

    public Account Account { get; }

    public void PerformOperation(decimal amount)
    {
        if (IsDeposit() == false)
            throw new ValidationException("You cannot withdraw from Credit account");
        Deposit(amount);
    }

    private bool IsDeposit()
    {
        return _type == TransactionType.Debit;
    }

    private void Deposit(decimal amount)
    {
        Validate(Account, amount);
        Account.Balance += amount;
    }

    private static void Validate(Account account, decimal amount)
    {
        ValidateAccount(account);
        ValidateAmount(amount);
    }

    private static void ValidateAccount(Account account)
    {
        if (account.ClosedAt != null && account.ClosedAt <= TimeUtils.GetTicksFromCurrentDate())
            throw new ValidationException("Error! Account is closed");
    }

    private static void ValidateAmount(decimal amount)
    {
        if (amount < 0)
            throw new ValidationException("Error! Amount less 0");
    }
}