using AccountService.Features.Accounts;
using Broker.AccountService;
using FluentValidation;

namespace AccountService.Features.Transactions.Utils.Balance;

public class CheckingBalance : IBalance
{

    // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local because checking account can't work with balance
    public CheckingBalance(Account account)
    {
        if (account.Type != AccountType.Checking)
            throw new ValidationException("Error! Invalid type for checking account");

        Account = account;
    }

    public Account Account { get; }

    public void PerformOperation(decimal amount)
    {
        throw new ValidationException("You cannot interact with the account being verified");
    }
}