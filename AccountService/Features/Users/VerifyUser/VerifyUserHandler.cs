using AccountService.Utils.Data;
using MediatR;

namespace AccountService.Features.Users.VerifyUser;

public class VerifyUserHandler : IRequestHandler<VerifyUserCommand, bool>
{
    private readonly DatabaseContext _database = DatabaseContext.Instance;

    public Task<bool> Handle(VerifyUserCommand request, CancellationToken cancellationToken)
    {
        var result = _database.CounterParties.Any(id => id == request.Id);
        return Task.FromResult(result);
    }
}