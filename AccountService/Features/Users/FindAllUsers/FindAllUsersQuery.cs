using MediatR;

namespace AccountService.Features.Users.FindAllUsers;

public class FindAllUsersQuery : IRequest<IEnumerable<User>?>
{
}