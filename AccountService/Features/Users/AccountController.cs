using AccountService.Features.Users.FindAllUsers;
using AccountService.Features.Users.VerifyUser;
using AccountService.Utils.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Features.Users;

[ApiController]
[Route("/users")]
[Produces("application/json")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Find all users
    /// </summary>
    /// <remarks>
    /// Find all ids users
    /// </remarks>
    /// <response code="200">The users find all</response>
    /// <response code="401">Unauthorized</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(MbResponse<List<User>>), 200)]
    public async Task<IActionResult> FindAllUsers()
    {
        var command = new FindAllUsersQuery();
        var users = await _mediator.Send(command);
        var result = ResultGenerator.Ok(users);
        return Ok(result);
    }

    /// <summary>
    /// Verify user
    /// </summary>
    /// <remarks>
    /// Check user by id
    /// </remarks>
    /// <response code="200">The user verify</response>
    /// <response code="401">Unauthorized</response>
    [HttpPost("{accountId}/verify")]
    [Authorize]
    [ProducesResponseType(typeof(MbResponse<bool>), 200)]
    public async Task<IActionResult> Verify(Guid accountId)
    {
        var command = new VerifyUserCommand(accountId);
        var result = await _mediator.Send(command);
        var response = ResultGenerator.Ok(result);
        return Ok(response);
    }
}