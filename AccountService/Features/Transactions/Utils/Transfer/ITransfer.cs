using AccountService.Features.Accounts;

namespace AccountService.Features.Transactions.Utils.Transfer;

public interface ITransfer
{
    void Validate();
    void CreateTransactionForAccountFrom(Transaction transaction, IAccountRepository repository);
    void CreateTransactionForAccountTo(Transaction transaction, IAccountRepository repository);
    void SwapTransactionsIds();
    Task<(Transaction, Transaction)> SaveToDatabaseAsync(ITransactionRepository repository);
}