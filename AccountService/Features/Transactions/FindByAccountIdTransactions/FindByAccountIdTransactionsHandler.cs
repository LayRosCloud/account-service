using AccountService.Features.Transactions.Dto;
using AccountService.Utils.Data;
using AccountService.Utils.Exceptions;
using AutoMapper;
using MediatR;

namespace AccountService.Features.Transactions.FindByAccountIdTransactions;

public class
    FindByAccountIdTransactionsHandler : IRequestHandler<FindByAccountIdTransactionsQuery, List<TransactionFullDto>>
{
    private readonly DatabaseContext _database = DatabaseContext.Instance;
    private readonly IMapper _mapper;

    public FindByAccountIdTransactionsHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Task<List<TransactionFullDto>> Handle(FindByAccountIdTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        var account = _database.Accounts.SingleOrDefault(x => x.Id == request.AccountId);
        if (account == null)
            throw ExceptionUtils.GetNotFoundException("account", request.AccountId);
        return Task.FromResult(_mapper.Map<List<TransactionFullDto>>(account.Transactions));
    }
}