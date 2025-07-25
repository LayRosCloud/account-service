using AccountService.Features.Users.FindAllUsers;
using AccountService.Features.Users.VerifyUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AccountService.Features.Users;

[ApiController]
[Route("/users")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Find all users",
        Description = "Find all users",
        OperationId = "FindAllUser",
        Tags = new[] { "User" }
    )]
    [SwaggerResponse(200, "The users find all", typeof(List<Guid>))]
    public async Task<IActionResult> FindAllUsers()
    {
        var command = new FindAllUsersQuery();
        var users = await _mediator.Send(command);
        return Ok(users);
    }

    [HttpPost("{accountId}/verify")]
    [SwaggerOperation(
        Summary = "Verify user",
        Description = "Check user by id",
        OperationId = "VerifyUser",
        Tags = new[] { "User" }
    )]
    [SwaggerResponse(200, "The user verify", typeof(bool))]
    public async Task<IActionResult> Verify(Guid accountId)
    {
        var command = new VerifyUserCommand(accountId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}