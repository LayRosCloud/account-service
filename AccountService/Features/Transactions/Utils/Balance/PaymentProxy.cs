using AccountService.Features.Accounts;

namespace AccountService.Features.Transactions.Utils.Balance;

public class PaymentProxy
{
    private readonly IBalance _balanceFrom;
    private readonly IBalance? _balanceTo;
    private readonly decimal _amount;

    public PaymentProxy(Transaction transaction, Account accountFrom, Account? accountTo)
    {
        if (transaction == null || accountFrom == null)
            throw new NullReferenceException("Transaction or Account is null");

        var type = transaction.Type;
        var factory = new BalanceFactory();

        _balanceFrom = factory.GetBalance(accountFrom, type);
        if (accountTo != null)
        {
            var inverseType = type == TransactionType.Debit ? TransactionType.Credit : TransactionType.Debit;
            _balanceTo = factory.GetBalance(accountTo, inverseType);
        }

        _amount = transaction.Sum;
    }

    public PaymentProxy(Transaction transaction, Account account) : this(transaction, account, null!)
    {
    }

    public void ExecuteTransaction()
    {
        _balanceFrom.PerformOperation(_amount);
        _balanceTo?.PerformOperation(_amount);
    }
}