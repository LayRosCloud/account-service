using AccountService.Broker;
using AccountService.Features.Accounts;
using AccountService.Features.Transactions.Utils.Balance;
using AccountService.Utils.Broker;
using AccountService.Utils.Data;
using AccountService.Utils.Time;
using Broker.AccountService;

namespace AccountService.Features.Transactions.DailyPercentAddToAccount;

public class DailyPercentAddedToAccount
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IStorageContext _storage;
    private readonly IProducer<InterestAccruedEvent> _producer;

    public DailyPercentAddedToAccount(ITransactionRepository repository, IAccountRepository accountRepository, IStorageContext storage, IProducer<InterestAccruedEvent> producer)
    {
        _transactionRepository = repository;
        _accountRepository = accountRepository;
        _storage = storage;
        _producer = producer;
    }

    public async Task AccrueInterest()
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
            var meta = MetaCreator.Create(Guid.NewGuid(), Guid.Parse("b367454a-efb0-4866-afca-9270bd9ed839"));
            var @event = new InterestAccruedEvent(Guid.NewGuid(), DateTime.UtcNow, meta)
            {
                Amount = transaction.Sum,
                PeriodFrom = DateTime.UtcNow.AddDays(-1),
                PeriodTo = DateTime.UtcNow.AddDays(1)
            };
            await _producer.ProduceAsync(@event);
        }

        await _transactionRepository.CreateRangeAsync(list.ToArray());
        await _storage.SaveChangesAsync();
    }
}