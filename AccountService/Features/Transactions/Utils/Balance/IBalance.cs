namespace AccountService.Features.Transactions.Utils.Balance;

public interface IBalance
{
    void PerformOperation(decimal amount);
}