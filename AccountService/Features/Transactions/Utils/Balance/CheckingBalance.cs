using FluentValidation;

namespace AccountService.Features.Transactions.Utils.Balance;

public class CheckingBalance : IBalance
{
    public void PerformOperation(decimal amount)
    {
        throw new ValidationException("You cannot interact with the account being verified");
    }
}