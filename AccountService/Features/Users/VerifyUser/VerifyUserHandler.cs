using AccountService.Utils.Data;
using MediatR;

namespace AccountService.Features.Users.VerifyUser;

// ReSharper disable once UnusedMember.Global using Mediator
public class VerifyUserHandler : IRequestHandler<VerifyUserCommand, bool>
{
    private readonly IDatabaseContext _database;

    public VerifyUserHandler(IDatabaseContext database)
    {
        _database = database;
    }

    public Task<bool> Handle(VerifyUserCommand request, CancellationToken cancellationToken)
    {
        var result = _database.CounterParties.Any(id => id == request.Id);
        return Task.FromResult(result);
    }
}