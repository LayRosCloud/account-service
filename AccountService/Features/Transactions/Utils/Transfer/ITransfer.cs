using AccountService.Utils.Data;

namespace AccountService.Features.Transactions.Utils.Transfer;

public interface ITransfer
{
    void Validate();
    void CreateTransactionForAccountFrom(Transaction transaction);
    void CreateTransactionForAccountTo(Transaction transaction);
    void SwapTransactionsIds();
    (Transaction, Transaction) SaveToDatabase(IDatabaseContext context);
}