using AccountService.Features.Accounts;
using AccountService.Utils.Data;

namespace AccountService.Features.Transactions.Utils.Transfer;

public class AccountTransaction
{
    public AccountTransaction(Account account)
    {
        Account = account;
    }

    public Account Account { get; }
    public Transaction Transaction { get; set; } = null!;

    public void AddToDatabase(IDatabaseContext context)
    {
        Account.Transactions.Add(Transaction);
        context.Transactions.Add(Transaction);
    }
}