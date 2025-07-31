using AccountService.Utils.Data;
using MediatR;

namespace AccountService.Features.Users.FindAllUsers;

public class FindAllUsersHandler : IRequestHandler<FindAllUsersQuery, List<Guid>>
{
    private readonly IDatabaseContext _database;

    public FindAllUsersHandler(IDatabaseContext database)
    {
        _database = database;
    }

    public Task<List<Guid>> Handle(FindAllUsersQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_database.CounterParties);
    }
}