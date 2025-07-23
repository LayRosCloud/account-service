using System.ComponentModel.DataAnnotations;
using AccountService.Features.Transactions.Dto;
using AccountService.Utils.Data;
using AccountService.Utils.Exceptions;
using AccountService.Utils.Time;
using AutoMapper;
using MediatR;

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
            throw new NotFoundException();

        if (account.ClosedAt != null)
            throw new ValidationException("Account is closed");

        var transaction = _mapper.Map<Transaction>(request);
        transaction.Currency = account.Currency;
        transaction.CreatedAt = TimeUtils.GetTicksFromCurrentDate();
        transaction.Id = new Guid();

        account.Balance += transaction.Type == TransactionType.Credit ? transaction.Sum : -transaction.Sum;
        
        account.Transactions.Add(transaction);
        _databaseContext.Transactions.Add(transaction);
        return Task.FromResult(_mapper.Map<TransactionFullDto>(transaction));
    }
}