using AccountService.Features.Users.Utils;
using MediatR;

namespace AccountService.Features.Users.FindAllUsers;

// ReSharper disable once UnusedMember.Global using Mediator
public class FindAllUsersHandler : IRequestHandler<FindAllUsersQuery, IEnumerable<User>?>
{
    private readonly IKeyCloakClient _client;

    public FindAllUsersHandler(IKeyCloakClient client)
    {
        _client = client;
    }

    public async Task<IEnumerable<User>?> Handle(FindAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _client.GetAllUsers();
        return users;
    }
}