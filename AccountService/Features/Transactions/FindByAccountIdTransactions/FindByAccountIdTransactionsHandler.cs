using AccountService.Features.Accounts;
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
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionWrapper _wrapper;
    private readonly IMapper _mapper;

    public FindByAccountIdTransactionsHandler(IMapper mapper, ITransactionWrapper wrapper, IAccountRepository accountRepository)
    {
        _mapper = mapper;
        _wrapper = wrapper;
        _accountRepository = accountRepository;
    }

    public async Task<List<TransactionFullDto>> Handle(FindByAccountIdTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        var transactions = await _wrapper.Execute(_ => FindAllAsync(request, cancellationToken), cancellationToken);
        return _mapper.Map<List<TransactionFullDto>>(transactions);
    }

    public async Task<List<Transaction>> FindAllAsync(FindByAccountIdTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        var account = await _accountRepository.FindByIdAsync(request.AccountId, true);

        if (account == null)
            throw ExceptionUtils.GetNotFoundException("account", request.AccountId);

        return account.Transactions.ToList();
    }
}