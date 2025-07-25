using AccountService.Features.Accounts;
using AccountService.Features.Transactions.Dto;
using AccountService.Utils.Data;
using AccountService.Utils.Exceptions;
using AccountService.Utils.Time;
using AutoMapper;
using MediatR;
using AccountService.Features.Transactions.Utils.Balance;

namespace AccountService.Features.Transactions.CreateTransaction;

public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, TransactionFullDto>
{
    private readonly DatabaseContext _databaseContext = DatabaseContext.Instance;
    private readonly IMapper _mapper;

    public CreateTransactionHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Task<TransactionFullDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var account = _databaseContext.Accounts.SingleOrDefault(acc => acc.Id == request.AccountId);

        if (account == null)
            throw ExceptionUtils.GetNotFoundException("Account", request.AccountId);

        var transaction = _mapper.Map<Transaction>(request);
        SetDefaultSettingTransaction(account, transaction);
        var proxy = new PaymentProxy(transaction, account);
        proxy.ExecuteTransaction();

        account.Transactions.Add(transaction);
        _databaseContext.Transactions.Add(transaction);
        return Task.FromResult(_mapper.Map<TransactionFullDto>(transaction));
    }

    private static void SetDefaultSettingTransaction(Account account, Transaction transaction)
    {
        transaction.Currency = account.Currency;
        transaction.CreatedAt = TimeUtils.GetTicksFromCurrentDate();
        transaction.Id = Guid.NewGuid();
    }
}