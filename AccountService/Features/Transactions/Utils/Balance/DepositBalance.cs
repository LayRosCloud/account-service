using AccountService.Features.Accounts;
using AccountService.Utils.Time;
using FluentValidation;

namespace AccountService.Features.Transactions.Utils.Balance;

public class DepositBalance : IBalance
{
    private readonly Account _account;
    private readonly TransactionType _type;

    public DepositBalance(Account account, TransactionType type)
    {
        _account = account;
        _type = type;
    }

    public void PerformOperation(decimal amount)
    {
        if(IsDeposit())
            Deposit(amount);
        else
            Withdraw(amount);
    }

    private bool IsDeposit()
    {
        return _type == TransactionType.Credit;
    }

    private void Withdraw(decimal amount)
    {
        Validate(_account, amount);
        if (_account.Balance - amount < 0)
            throw new ValidationException("The account balance is insufficient");

        _account.Balance -= amount;
    }

    private void Deposit(decimal amount)
    {
        Validate(_account, amount);
        _account.Balance += amount;
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