using MediatR;

namespace AccountService.Features.Users.VerifyUser;

public class VerifyUserCommand : IRequest<bool>
{
    public VerifyUserCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}