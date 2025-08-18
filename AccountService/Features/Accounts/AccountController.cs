using AccountService.Features.Accounts.CreateAccount;
using AccountService.Features.Accounts.DeleteAccount;
using AccountService.Features.Accounts.Dto;
using AccountService.Features.Accounts.FindAllAccounts;
using AccountService.Features.Accounts.FindByIdAccount;
using AccountService.Features.Accounts.FindByIdAccountExtract;
using AccountService.Features.Accounts.HasAccountWithCounterParty;
using AccountService.Features.Accounts.UpdatePercentAccount;
using AccountService.Features.Accounts.UpdateTypeAccount;
using AccountService.Utils.Broker;
using AccountService.Utils.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Features.Accounts;

[ApiController]
[Route("/accounts")]
[Produces("application/json")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Finds all accounts
    /// </summary>
    /// <remarks>
    /// finds all accounts without transactions
    /// </remarks>
    /// <response code="200">returns a list of accounts</response>
    /// <response code="401">Unauthorized</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(MbResponse<List<AccountResponseShortDto>>), 200)]
    public async Task<IActionResult> FindAllAccounts()
    {
        var query = new FindAllAccountsQuery();
        var accounts = await _mediator.Send(query);
        var response = ResultGenerator.Ok(accounts);
        CausationHandler.ChangeCautionHeader(HttpContext, Guid.Parse("3130605b-8d5d-446e-bc06-55b44d38476a"));
        return Ok(response);
    }

    /// <summary>
    /// Check exist account
    /// </summary>
    /// <remarks>
    /// if the account and the account holder exist in combination - true, else - false
    /// </remarks>
    /// <param name="accountId">account id</param>
    /// <param name="userId">user id</param>
    /// <response code="200">does this user have an account?</response>
    /// <response code="401">Unauthorized</response>
    [HttpGet("{accountId}/users/{userId}")]
    [Authorize]
    [ProducesResponseType(typeof(MbResponse<bool>), 200)]
    public async Task<IActionResult> HasAccountWithCounterPartyId(
        Guid accountId,
        Guid userId)
    {
        var query = new HasAccountWithCounterPartyCommand(userId, accountId);
        var result = await _mediator.Send(query);
        var response = ResultGenerator.Ok(result);
        CausationHandler.ChangeCautionHeader(HttpContext, Guid.Parse("d28ecaab-8595-46a9-a308-0c9f07c01fd5"));
        return Ok(response);
    }

    /// <summary>
    /// Finds by id account
    /// </summary>
    /// <remarks>
    /// finds by account with transactions between two dates or by id
    /// </remarks>
    /// <param name="accountId">Account id</param>
    /// <param name="dateStart">Date start transactions</param>
    /// <param name="dateEnd">Date end transactions</param>
    /// <response code="200">Finds by id account</response>
    /// <response code="400">AccountId is empty or bad format UUID</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Account with id is not found</response>
    [HttpGet("{accountId}")]
    [Authorize]
    [ProducesResponseType(typeof(MbResponse<AccountResponseFullDto>), 200)]
    [ProducesResponseType(typeof(MbError), 400)]
    [ProducesResponseType(typeof(MbError), 404)]
    public async Task<IActionResult> FindByAccountIdExtract(
        Guid accountId,
        DateTimeOffset? dateStart = null,
        DateTimeOffset? dateEnd = null)
    {
        if (dateStart.HasValue && dateEnd.HasValue)
        {
            var query = new FindByIdAccountExtractQuery(accountId, dateStart.Value, dateEnd.Value);
            var account = await _mediator.Send(query);
            var response = ResultGenerator.Ok(account);
            CausationHandler.ChangeCautionHeader(HttpContext, Guid.Parse("b4a5aac6-7ab5-44e9-9fea-e01eb80a85c4"));
            return Ok(response);
        }

        var queryId = new FindByIdAccountQuery(accountId);
        var accountResponse = await _mediator.Send(queryId);
        var responseId = ResultGenerator.Ok(accountResponse);
        CausationHandler.ChangeCautionHeader(HttpContext, Guid.Parse("b4a5aac6-7ab5-44e9-9fea-e01eb80a85c4"));
        return Ok(responseId);
    }

    /// <summary>
    /// Create account
    /// </summary>
    /// <remarks>
    /// Create account for client
    /// </remarks>
    /// <response code="201">Create account</response>
    /// <response code="400">Account data is invalid</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Account or other field with id is not found</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(MbResponse<AccountResponseShortDto>), 201)]
    [ProducesResponseType(typeof(MbError), 400)]
    [ProducesResponseType(typeof(MbError), 404)]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand command)
    {
        var account = await _mediator.Send(command);
        var response = ResultGenerator.Create(account);
        CausationHandler.ChangeCautionHeader(HttpContext, MetaCreator.AccountCreate);
        return new ObjectResult(response) { StatusCode = 201 };
    }

    /// <summary>
    /// Updates a percent account
    /// </summary>
    /// <remarks>
    /// Updates account a percent for client
    /// </remarks>
    /// <response code="200">Patch by id a percent field account</response>
    /// <response code="400">Account data is invalid</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Account or other field with id is not found</response>
    /// <response code="409">Account will be changed</response>
    [HttpPatch("{accountId}/percent")]
    [Authorize]
    [ProducesResponseType(typeof(MbResponse<AccountResponseShortDto>), 200)]
    [ProducesResponseType(typeof(MbError), 400)]
    [ProducesResponseType(typeof(MbError), 404)]
    [ProducesResponseType(typeof(MbError), 409)]
    public async Task<IActionResult> UpdatePercentAccount([FromBody] UpdateAccountPercentCommand percentCommand,
        Guid accountId)
    {
        percentCommand.Id = accountId;
        var account = await _mediator.Send(percentCommand);
        var response = ResultGenerator.Ok(account);
        CausationHandler.ChangeCautionHeader(HttpContext, Guid.Parse("d2234fa5-1685-40fb-aa6a-c7d17e878d74"));
        return Ok(response);
    }

    /// <summary>
    /// Updates a type account
    /// </summary>
    /// <remarks>
    /// Updates account a type for client
    /// </remarks>
    /// <response code="200">Patch by id a type field account</response>
    /// <response code="400">Account data is invalid</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Account or other field with id is not found</response>
    /// <response code="409">Account will be changed</response>
    [HttpPatch("{accountId}/type")]
    [Authorize]
    [ProducesResponseType(typeof(MbResponse<AccountResponseShortDto>), 200)]
    [ProducesResponseType(typeof(MbError), 400)]
    [ProducesResponseType(typeof(MbError), 404)]
    [ProducesResponseType(typeof(MbError), 409)]
    public async Task<IActionResult> UpdateTypeAccount([FromBody] UpdateAccountTypeCommand accountTypeCommand,
        Guid accountId)
    {
        accountTypeCommand.AccountId = accountId;
        var account = await _mediator.Send(accountTypeCommand);
        var result = ResultGenerator.Ok(account);
        CausationHandler.ChangeCautionHeader(HttpContext, Guid.Parse("08639a4b-d0c0-41a0-ba1e-08cddbf3f283"));
        return Ok(result);
    }

    /// <summary>
    /// Deletes account
    /// </summary>
    /// <remarks>
    /// Deletes account by id
    /// </remarks>
    /// <param name="accountId">account id</param>
    /// <response code="200">Delete by id account</response>
    /// <response code="400">AccountId is empty or bad format UUID</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Account or other field with id is not found</response>
    /// <response code="409">Account will be changed</response>
    [HttpDelete("{accountId}")]
    [Authorize]
    [ProducesResponseType(typeof(MbResponse<Unit>), 200)]
    [ProducesResponseType(typeof(MbError), 400)]
    [ProducesResponseType(typeof(MbError), 404)]
    [ProducesResponseType(typeof(MbError), 409)]
    public async Task<IActionResult> DeleteAccount(Guid accountId)
    {
        var command = new DeleteAccountCommand(accountId);
        var account = await _mediator.Send(command);
        var response = ResultGenerator.Ok(account);
        CausationHandler.ChangeCautionHeader(HttpContext, Guid.Parse("f39fe0af-39f0-45f5-8867-9b3eb8582534"));
        return Ok(response);
    }
}