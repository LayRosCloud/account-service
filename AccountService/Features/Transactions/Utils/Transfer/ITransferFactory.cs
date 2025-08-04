using AccountService.Features.Accounts;
using AccountService.Features.Transactions.TransferBetweenAccounts;

namespace AccountService.Features.Transactions.Utils.Transfer;

public interface ITransferFactory
{
    ITransfer GetTransfer(Account account1, Account account2, TransferBetweenAccountsCommand request);
}