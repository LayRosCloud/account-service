using AccountService.Utils.Data;
using AccountService.Utils.Exceptions;
using AccountService.Utils.Time;
using MediatR;

namespace AccountService.Features.Accounts.DeleteAccount;

// ReSharper disable once UnusedMember.Global using Mediator
public class DeleteAccountHandler : IRequestHandler<DeleteAccountCommand, Unit>
{
    private readonly IStorageContext _storage;
    private readonly IAccountRepository _repository;
    private readonly ITransactionWrapper _wrapper;

    public DeleteAccountHandler(IStorageContext storage, IAccountRepository repository, ITransactionWrapper wrapper)
    {
        _storage = storage;
        _repository = repository;
        _wrapper = wrapper;
    }

    public async Task<Unit> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        await _wrapper.Execute(_ => UpdateAsync(request, cancellationToken), cancellationToken);
        return Unit.Value;
    }

    private async Task<Account> UpdateAsync(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await GetAccountIfExists(request.AccountId);

        account.ClosedAt = account.ClosedAt == null ? TimeUtils.GetTicksFromCurrentDate() : null;
        _repository.Update(account);
        await _storage.SaveChangesAsync(cancellationToken);
        return account;
    }

    private async Task<Account> GetAccountIfExists(Guid accountId)
    {
        var account = await _repository.FindByIdAsync(accountId);
        if (account == null)
            throw ExceptionUtils.GetNotFoundException("Account", accountId);
        return account;
    }
}