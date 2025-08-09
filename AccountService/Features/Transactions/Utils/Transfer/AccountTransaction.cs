using AccountService.Features.Accounts;

namespace AccountService.Features.Transactions.Utils.Transfer;

public class AccountTransaction
{
    public AccountTransaction(Account account)
    {
        Account = account;
    }

    public Account Account { get; }
    public Transaction Transaction { get; set; } = null!;
}