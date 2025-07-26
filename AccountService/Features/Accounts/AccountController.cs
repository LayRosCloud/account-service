using AccountService.Features.Accounts.CreateAccount;
using AccountService.Features.Accounts.DeleteAccount;
using AccountService.Features.Accounts.Dto;
using AccountService.Features.Accounts.FindAllAccounts;
using AccountService.Features.Accounts.FindByIdAccount;
using AccountService.Features.Accounts.FindByIdAccountExtract;
using AccountService.Features.Accounts.HasAccountWithCounterParty;
using AccountService.Features.Accounts.UpdatePercentAccount;
using AccountService.Features.Accounts.UpdateTypeAccount;
using AccountService.Utils.Middleware;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AccountService.Features.Accounts;

[ApiController]
[Route("/accounts")]
[SwaggerTag("accounts of users")]
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

    [HttpGet("{accountId}/users/{userId}")]
    [SwaggerOperation(
        Summary = "Check exist account",
        Description = "if the account and the account holder exist in combination - true, else - false",
        OperationId = "HasAccountWithCounterPartyId",
        Tags = new[] { "Account" }
    )]
    [SwaggerResponse(200, "does this user have an account?", typeof(bool))]
    public async Task<IActionResult> HasAccountWithCounterPartyId(
        [SwaggerParameter("account id", Required = true)]
        Guid accountId,
        [SwaggerParameter("user id", Required = true)]
        Guid userId)
    {
        var query = new HasAccountWithCounterPartyCommand(userId, accountId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{accountId}/extract")]
    [SwaggerOperation(
        Summary = "Finds by id account",
        Description = "finds by account with transactions between two dates",
        OperationId = "FindByIdAccountExtract",
        Tags = new[] { "Account" }
    )]
    [SwaggerResponse(200, "Finds by id account", typeof(AccountResponseFullDto))]
    [SwaggerResponse(400, "AccountId is empty or bad format UUID", typeof(ExceptionDto))]
    [SwaggerResponse(404, "Account with id is not found", typeof(ExceptionDto))]
    public async Task<IActionResult> FindByAccountIdExtract(
        [SwaggerParameter("account id", Required = true)]
        Guid accountId,
        [SwaggerParameter("date start in ticks", Required = true)]
        DateTime dateStart,
        [SwaggerParameter("date end in ticks", Required = true)]
        DateTime dateEnd)
    {
        var query = new FindByIdAccountExtractQuery(accountId, dateStart.Ticks, dateEnd.Ticks);
        var account = await _mediator.Send(query);
        return Ok(account);
    }

    [HttpGet("{accountId}")]
    [SwaggerOperation(
        Summary = "Finds by id account",
        Description = "finds by account with transactions",
        OperationId = "FindByIdAccount",
        Tags = new[] { "Account" }
    )]
    [SwaggerResponse(200, "Finds by id account", typeof(AccountResponseFullDto))]
    [SwaggerResponse(400, "AccountId is empty or bad format UUID", typeof(ExceptionDto))]
    [SwaggerResponse(404, "Account with id is not found", typeof(ExceptionDto))]
    public async Task<IActionResult> FindByAccountIdExtract(
        [SwaggerParameter("account id", Required = true)]
        Guid accountId)
    {
        var query = new FindByIdAccountQuery(accountId);
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
    [SwaggerResponse(201, "Finds by id account", typeof(AccountResponseShortDto))]
    [SwaggerResponse(400, "Account data is invalid", typeof(ExceptionDto))]
    [SwaggerResponse(404, "Account or other field with id is not found", typeof(ExceptionDto))]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand command)
    {
        var account = await _mediator.Send(command);
        return new ObjectResult(account) { StatusCode = 201 };
    }

    [HttpPatch("{accountId}/percent")]
    [SwaggerOperation(
        Summary = "Updates a percent account",
        Description = "Updates account a percent for client",
        OperationId = "UpdatePercentAccount",
        Tags = new[] { "Account" }
    )]
    [SwaggerResponse(200, "Finds by id a percent field account", typeof(AccountResponseShortDto))]
    [SwaggerResponse(400, "Account data is invalid", typeof(ExceptionDto))]
    [SwaggerResponse(404, "Account or other field with id is not found", typeof(ExceptionDto))]
    public async Task<IActionResult> UpdatePercentAccount([FromBody] UpdateAccountPercentCommand percentCommand,
        Guid accountId)
    {
        percentCommand.Id = accountId;
        var account = await _mediator.Send(percentCommand);
        return Ok(account);
    }

    [HttpPatch("{accountId}/type")]
    [SwaggerOperation(
        Summary = "Updates a type account",
        Description = "Updates account a type for client",
        OperationId = "UpdateTypeAccount",
        Tags = new[] { "Account" }
    )]
    [SwaggerResponse(200, "Finds by id a type field account", typeof(AccountResponseShortDto))]
    [SwaggerResponse(400, "Account data is invalid", typeof(ExceptionDto))]
    [SwaggerResponse(404, "Account or other field with id is not found", typeof(ExceptionDto))]
    public async Task<IActionResult> UpdateTypeAccount([FromBody] UpdateTypeAccountCommand typeCommand,
        Guid accountId)
    {
        typeCommand.AccountId = accountId;
        var account = await _mediator.Send(typeCommand);
        return Ok(account);
    }

    [HttpDelete("{accountId}")]
    [SwaggerOperation(
        Summary = "Deletes account",
        Description = "Deletes account",
        OperationId = "DeleteAccount",
        Tags = new[] { "Account" }
    )]
    [SwaggerResponse(200, "Finds by id account", typeof(Unit))]
    [SwaggerResponse(400, "AccountId is empty or bad format UUID", typeof(ExceptionDto))]
    [SwaggerResponse(404, "Account with id is not found", typeof(ExceptionDto))]
    public async Task<IActionResult> DeleteAccount([SwaggerParameter("account id", Required = true)] Guid accountId)
    {
        var command = new DeleteAccountCommand(accountId);
        var account = await _mediator.Send(command);
        return Ok(account);
    }
}