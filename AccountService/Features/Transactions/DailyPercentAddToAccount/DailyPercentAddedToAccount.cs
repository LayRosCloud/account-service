using AccountService.Features.Accounts;
using AccountService.Features.Transactions.Utils.Balance;
using AccountService.Utils.Data;
using AccountService.Utils.Time;

namespace AccountService.Features.Transactions.DailyPercentAddToAccount;

public class DailyPercentAddedToAccount
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IStorageContext _storage;

    public DailyPercentAddedToAccount(ITransactionRepository repository, IAccountRepository accountRepository, IStorageContext storage)
    {
        _transactionRepository = repository;
        _accountRepository = accountRepository;
        _storage = storage;
    }

    public async Task DailyPercentAsync()
    {
        var accounts = await _accountRepository.FindAllByPercentNotNullAndNotClosedAtAsync();
        var list = new List<Transaction>();
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var account in accounts)
        {
            var rate = account.Percent!.Value;
            var date = DateTimeOffset.Now - account.ClosedAt!.Value;
            var amount = account.Balance * (rate / 100) * ((decimal)date.TotalDays / 365);
            var transaction = new Transaction()
            {
                AccountId = account.Id,
                CreatedAt = TimeUtils.GetTicksFromCurrentDate(),
                Currency = account.Currency,
                Description = "Percent add",
                Sum = amount,
                Type = TransactionType.Debit,
                Id = Guid.NewGuid()
            };
            list.Add(transaction);
            var proxy = new PaymentBalance(transaction, account);
            proxy.ExecuteTransactionAsync(_accountRepository);
        }

        await _transactionRepository.CreateRangeAsync(list.ToArray());
        await _storage.SaveChangesAsync();
    }
}