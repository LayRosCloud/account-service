using AccountService.Features.Accounts.Dto;
using AccountService.Features.Transactions;
using AccountService.Features.Transactions.Dto;
using AccountService.Features.Transactions.Utils.Balance;
using AccountService.Utils.Data;
using AccountService.Utils.Exceptions;
using AutoMapper;
using MediatR;

namespace AccountService.Features.Accounts.FindByIdAccountExtract;

public class FindByIdAccountExtractHandler : IRequestHandler<FindByIdAccountExtractQuery, AccountResponseFullDto>
{
    private readonly IDatabaseContext _database;
    private readonly IMapper _mapper;

    public FindByIdAccountExtractHandler(IMapper mapper, IDatabaseContext database)
    {
        _mapper = mapper;
        _database = database;
    }

    public Task<AccountResponseFullDto> Handle(FindByIdAccountExtractQuery request, CancellationToken cancellationToken)
    {
        var account = _database.Accounts.FirstOrDefault(acc => acc.Id == request.AccountId);
        if (account == null) throw ExceptionUtils.GetNotFoundException("Account", request.AccountId);

        var balanceBefore = account.Transactions.Where(x => x.CreatedAt < request.DateStart).Sum(x => x.Sum);
        var result = _mapper.Map<AccountResponseFullDto>(account);
        result.Balance = balanceBefore;

        var transactions = new LinkedList<TransactionFullDto>();
        var clone = (Account)account.Clone();
        clone.Balance = balanceBefore;
        foreach (var transaction in result.Transactions)
            if (request.DateStart <= transaction.CreatedAt && transaction.CreatedAt <= request.DateEnd)
            {
                var trans = _mapper.Map<Transaction>(transaction);
                var proxy = new PaymentProxy(trans, clone);
                proxy.ExecuteTransaction();
                transactions.AddLast(transaction);
            }

        result.Balance = clone.Balance;
        result.Transactions = transactions.ToList();

        return Task.FromResult(_mapper.Map<AccountResponseFullDto>(result));
    }
}