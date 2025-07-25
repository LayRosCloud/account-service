using AccountService.Utils.Data;
using MediatR;

namespace AccountService.Features.Users.FindAllUsers;

public class FindAllUsersHandler : IRequestHandler<FindAllUsersQuery, List<Guid>>
{
    private readonly DatabaseContext _databaseContext = DatabaseContext.Instance;

    public Task<List<Guid>> Handle(FindAllUsersQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_databaseContext.CounterParties);
    }
}