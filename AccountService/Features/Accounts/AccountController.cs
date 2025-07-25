using AccountService.Features.Accounts.CreateAccount;
using AccountService.Features.Accounts.DeleteAccount;
using AccountService.Features.Accounts.Dto;
using AccountService.Features.Accounts.FindAllAccounts;
using AccountService.Features.Accounts.FindByIdAccount;
using AccountService.Features.Accounts.UpdatePercentAccount;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AccountService.Features.Accounts;

[ApiController]
[Route("/accounts")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Finds all accounts",
        Description = "finds all accounts without transactions",
        OperationId = "FindAllAccounts",
        Tags = new[] { "Account" }
    )]
    [SwaggerResponse(200, "Finds all accounts", typeof(List<AccountResponseShortDto>))]
    public async Task<IActionResult> FindAllAccounts()
    {
        var query = new FindAllAccountsQuery();
        var account = await _mediator.Send(query);
        return Ok(account);
    }

    [HttpGet("{accountId}")]
    [SwaggerOperation(
        Summary = "Finds by id account",
        Description = "finds by  accounts without transactions",
        OperationId = "FindByIdAccount",
        Tags = new[] { "Account" }
    )]
    [SwaggerResponse(200, "Finds by id account", typeof(AccountResponseFullDto))]
    [SwaggerResponse(400, "AccountId is empty or bad format UUID")]
    [SwaggerResponse(404, "Account with id is not found")]
    public async Task<IActionResult> FindByAccountId(
        [SwaggerParameter("account id", Required = true)] Guid accountId,
        long dateStart, long dateEnd)
    {
        var query = new FindByIdAccountQuery(accountId, dateStart, dateEnd);
        var account = await _mediator.Send(query);
        return Ok(account);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create account",
        Description = "Create account for client ",
        OperationId = "CreateAccount",
        Tags = new[] { "Account" }
    )]
    [SwaggerResponse(200, "Finds by id account", typeof(AccountResponseShortDto))]
    [SwaggerResponse(400, "Account data is invalid")]
    [SwaggerResponse(404, "Account or other field with id is not found")]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand command)
    {
        var account = await _mediator.Send(command);
        return Ok(account);
    }

    [HttpPatch("{accountId}/percent")]
    [SwaggerOperation(
        Summary = "Updates account",
        Description = "Updates account for client",
        OperationId = "UpdateAccount",
        Tags = new[] { "Account" }
    )]
    [SwaggerResponse(200, "Finds by id account", typeof(AccountResponseShortDto))]
    [SwaggerResponse(400, "Account data is invalid")]
    [SwaggerResponse(404, "Account or other field with id is not found")]
    public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountCommand command, Guid accountId)
    {
        command.Id = accountId;
        var account = await _mediator.Send(command);
        return Ok(account);
    }

    [HttpDelete("{accountId}")]
    [SwaggerOperation(
        Summary = "Deletes account",
        Description = "Deletes account",
        OperationId = "DeleteAccount",
        Tags = new[] { "Account" }
    )]
    [SwaggerResponse(200, "Finds by id account", typeof(AccountResponseShortDto))]
    [SwaggerResponse(400, "AccountId is empty or bad format UUID")]
    [SwaggerResponse(404, "Account with id is not found")]
    public async Task<IActionResult> DeleteAccount([SwaggerParameter("account id", Required = true)] Guid accountId)
    {
        var command = new DeleteAccountCommand(accountId);
        var account = await _mediator.Send(command);
        return Ok(account);
    }
}