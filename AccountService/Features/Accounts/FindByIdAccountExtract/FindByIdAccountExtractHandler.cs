using AccountService.Features.Accounts.Dto;
using AccountService.Features.Transactions;
using AccountService.Features.Transactions.Dto;
using AccountService.Features.Transactions.Utils.Balance;
using AccountService.Utils.Exceptions;
using AutoMapper;
using MediatR;

namespace AccountService.Features.Accounts.FindByIdAccountExtract;

// ReSharper disable once UnusedMember.Global using Mediator
public class FindByIdAccountExtractHandler : IRequestHandler<FindByIdAccountExtractQuery, AccountResponseFullDto>
{
    private readonly IAccountRepository _repository;
    private readonly IMapper _mapper;

    public FindByIdAccountExtractHandler(IMapper mapper, IAccountRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<AccountResponseFullDto> Handle(FindByIdAccountExtractQuery request, CancellationToken cancellationToken)
    {
        var account = await ExistsAccountAsync(request.AccountId);

        var balance = account.AccountTransactions.Where(x => x.CreatedAt < request.DateStart).Sum(x => x.Sum);
        var result = _mapper.Map<AccountResponseFullDto>(account);
        result.Balance = balance;

        var transactions = new LinkedList<Transaction>();
        var clone = (Account)account.Clone();
        clone.Balance = balance;
        foreach (var transaction in clone.AccountTransactions)
            if (InBetweenTwoDates(request, transaction.CreatedAt))
            {
                var transactionTemp = _mapper.Map<Transaction>(transaction);
                var proxy = new PaymentBalance(transactionTemp, clone);
                proxy.ExecuteTransactionAsync();
                transactions.AddLast(transaction);
            }

        result.Balance = clone.Balance;
        result.Transactions = _mapper.Map<List<TransactionFullDto>>(transactions);

        return _mapper.Map<AccountResponseFullDto>(result);
    }

    private async Task<Account> ExistsAccountAsync(Guid accountId)
    {
        var account = await _repository.FindByIdAsync(accountId, true);
        if (account == null)
            throw ExceptionUtils.GetNotFoundException("Account", accountId);
        return account;
    }

    private static bool InBetweenTwoDates(FindByIdAccountExtractQuery request, DateTimeOffset createdAt)
    {
        return request.DateStart <= createdAt && createdAt <= request.DateEnd;
    }
}