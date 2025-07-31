using AccountService.Features.Accounts;
using AccountService.Features.Transactions.Dto;
using AccountService.Features.Transactions.Utils.Balance;
using AccountService.Utils.Data;
using AccountService.Utils.Exceptions;
using AccountService.Utils.Time;
using AutoMapper;
using MediatR;

namespace AccountService.Features.Transactions.CreateTransaction;

// ReSharper disable once UnusedMember.Global using Mediator
public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, TransactionFullDto>
{
    private readonly IDatabaseContext _database;
    private readonly IMapper _mapper;

    public CreateTransactionHandler(IMapper mapper, IDatabaseContext database)
    {
        _mapper = mapper;
        _database = database;
    }

    public Task<TransactionFullDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var account = _database.Accounts.SingleOrDefault(acc => acc.Id == request.AccountId);

        if (account == null)
            throw ExceptionUtils.GetNotFoundException("Account", request.AccountId);

        var transaction = _mapper.Map<Transaction>(request);
        SetDefaultSettingTransaction(account, transaction);
        var proxy = new PaymentBalance(transaction, account);
        proxy.ExecuteTransaction();

        account.Transactions.Add(transaction);
        _database.Transactions.Add(transaction);
        return Task.FromResult(_mapper.Map<TransactionFullDto>(transaction));
    }

    private static void SetDefaultSettingTransaction(Account account, Transaction transaction)
    {
        transaction.Currency = account.Currency;
        transaction.CreatedAt = TimeUtils.GetTicksFromCurrentDate();
        transaction.Id = Guid.NewGuid();
    }
}