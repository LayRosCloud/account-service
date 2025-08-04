using AccountService.Features.Users.Utils;
using MediatR;

namespace AccountService.Features.Users.VerifyUser;

// ReSharper disable once UnusedMember.Global using Mediator
public class VerifyUserHandler : IRequestHandler<VerifyUserCommand, bool>
{
    private readonly IKeyCloakClient _client;

    public VerifyUserHandler(IKeyCloakClient client)
    {
        _client = client;
    }

    public async Task<bool> Handle(VerifyUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _client.FindUser(request.Id);
        return user != null;
    }
}