using AccountService.Features.Accounts.Dto;
using AccountService.Features.Transactions;
using AccountService.Features.Transactions.Dto;
using AccountService.Features.Transactions.Utils.Balance;
using AccountService.Utils.Data;
using AccountService.Utils.Exceptions;
using AutoMapper;
using MediatR;

namespace AccountService.Features.Accounts.FindByIdAccount;

public class FindByIdAccountHandler : IRequestHandler<FindByIdAccountQuery, AccountResponseFullDto>
{
    private readonly DatabaseContext _databaseContext = DatabaseContext.Instance;
    private readonly IMapper _mapper;

    public FindByIdAccountHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Task<AccountResponseFullDto> Handle(FindByIdAccountQuery request, CancellationToken cancellationToken)
    {
        var account = _databaseContext.Accounts.FirstOrDefault(acc => acc.Id == request.AccountId);
        if (account == null) throw ExceptionUtils.GetNotFoundException("Account", request.AccountId);

        var balanceBefore = account.Transactions.Where(x => x.CreatedAt < request.DateStart).Sum(x => x.Sum);
        var result = _mapper.Map<AccountResponseFullDto>(account);
        result.Balance = balanceBefore;
        
        var transactions = new LinkedList<TransactionFullDto>();

        foreach (var transaction in result.Transactions)
        {
            if (request.DateStart < transaction.CreatedAt || transaction.CreatedAt > request.DateEnd) continue;

            var proxy = new PaymentProxy(_mapper.Map<Transaction>(transaction), account);
            proxy.ExecuteTransaction();
            transactions.AddLast(transaction);

        }

        result.Transactions = transactions.ToList();

        return Task.FromResult(_mapper.Map<AccountResponseFullDto>(account));
    }
}