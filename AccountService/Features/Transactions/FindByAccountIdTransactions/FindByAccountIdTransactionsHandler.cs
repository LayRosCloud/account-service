using AccountService.Features.Transactions.Dto;
using AccountService.Utils.Data;
using AccountService.Utils.Exceptions;
using AutoMapper;
using MediatR;

namespace AccountService.Features.Transactions.FindByAccountIdTransactions;

// ReSharper disable once UnusedMember.Global using Mediator
public class FindByAccountIdTransactionsHandler :
    IRequestHandler<FindByAccountIdTransactionsQuery, List<TransactionFullDto>>
{
    private readonly IDatabaseContext _database;
    private readonly IMapper _mapper;

    public FindByAccountIdTransactionsHandler(IMapper mapper, IDatabaseContext database)
    {
        _mapper = mapper;
        _database = database;
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