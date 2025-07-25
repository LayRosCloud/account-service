using AccountService.Features.Accounts;

namespace AccountService.Features.Transactions.Utils.Balance;

public class BalanceFactory
{
    public IBalance GetBalance(Account account, TransactionType type)
    {
        return account.Type switch
        {
            AccountType.Checking => new CheckingBalance(),
            AccountType.Credit => new CreditBalance(account, type),
            AccountType.Deposit => new DepositBalance(account, type),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}