using AccountService.Features.Accounts;

namespace AccountService.Features.Transactions.Utils.Balance;

public class PaymentBalance
{
    private readonly decimal _amount;
    private readonly IBalance _balance;

    public PaymentBalance(Transaction transaction, Account account)
    {
        if (transaction == null || account == null)
            throw new NullReferenceException("Transaction or Account is null");

        var factory = new BalanceFactory();
        _balance = factory.GetBalanceHandler(account, transaction.Type);

        _amount = transaction.Sum;
    }

    public void ExecuteTransaction()
    {
        _balance.PerformOperation(_amount);
    }
}