using AccountService.Features.Accounts.CreateAccount;
using AccountService.Features.Accounts.DeleteAccount;
using AccountService.Features.Accounts.FindAllAccounts;
using AccountService.Features.Accounts.FindByIdAccount;
using AccountService.Features.Accounts.UpdateAccount;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> FindAllAccounts()
    {
        var query = new FindAllAccountsQuery();
        var account = await _mediator.Send(query);
        return Ok(account);
    }

    [HttpGet("{accountId}")]
    public async Task<IActionResult> FindByAccountId(Guid accountId)
    {
        var query = new FindByIdAccountQuery(accountId);
        var account = await _mediator.Send(query);
        return Ok(account);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand command)
    {
        var account = await _mediator.Send(command);
        return Ok(account);
    }

    [HttpPut("{accountId}")]
    public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountCommand command, Guid accountId)
    {
        command.Id = accountId;
        var account = await _mediator.Send(command);
        return Ok(account);
    }

    [HttpDelete("{accountId}")]
    public async Task<IActionResult> DeleteAccount(Guid accountId)
    {
        var command = new DeleteAccountCommand(accountId);
        var account = await _mediator.Send(command);
        return Ok(account);
    }
}