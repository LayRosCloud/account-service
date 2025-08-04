using AccountService.Features.Accounts;
using AccountService.Features.Accounts.FindByIdAccount.Internal;
using AccountService.Features.Transactions.Dto;
using AccountService.Features.Transactions.Utils.Transfer;
using AccountService.Utils.Data;
using AutoMapper;
using MediatR;

namespace AccountService.Features.Transactions.TransferBetweenAccounts;

// ReSharper disable once UnusedMember.Global using Mediator
public class TransferBetweenAccountsHandler : IRequestHandler<TransferBetweenAccountsCommand, TransactionFullDto>
{
    private readonly IDatabaseContext _database;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ITransferFactory _factory;

    public TransferBetweenAccountsHandler(IMapper mapper, IMediator mediator, IDatabaseContext database, ITransferFactory factory)
    {
        _mapper = mapper;
        _mediator = mediator;
        _database = database;
        _factory = factory;
    }

    public async Task<TransactionFullDto> Handle(TransferBetweenAccountsCommand request, CancellationToken cancellationToken)
    {
        var (accountFrom, accountTo) = await FindByIds(request.AccountId, request.CounterPartyAccountId);
        var transferHandler = _factory.GetTransfer(accountFrom, accountTo, request);

        transferHandler.Validate();

        transferHandler.CreateTransactionForAccountFrom(_mapper.Map<Transaction>(request));
        transferHandler.CreateTransactionForAccountTo(_mapper.Map<Transaction>(request));

        transferHandler.SwapTransactionsIds();

        var (transactionFrom, _) = transferHandler.SaveToDatabase(_database);
        return _mapper.Map<TransactionFullDto>(transactionFrom);
    }

    private async Task<(Account, Account)> FindByIds(Guid id1, Guid id2)
    {
        var account1 = await FindByIdAccount(id1);
        var account2 = await FindByIdAccount(id2);
        return (account1, account2);
    }

    private async Task<Account> FindByIdAccount(Guid id)
    {
        var query = new FindByIdAccountInternalQuery(id);
        return _mapper.Map<Account>(await _mediator.Send(query));
    }
}