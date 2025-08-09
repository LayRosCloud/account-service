using AccountService.Features.Accounts;

namespace AccountService.Features.Transactions.Utils.Balance;

public interface IBalance
{
    public Account Account { get; }
    void PerformOperation(decimal amount);
}