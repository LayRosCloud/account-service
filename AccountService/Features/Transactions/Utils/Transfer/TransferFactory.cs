using AccountService.Features.Accounts;
using AccountService.Features.Transactions.TransferBetweenAccounts;

namespace AccountService.Features.Transactions.Utils.Transfer;

public class TransferFactory : ITransferFactory
{
    public ITransfer GetTransfer(Account account1, Account account2, TransferBetweenAccountsCommand request)
    {
        return new TransferHandler(account1, account2, request);
    }
}