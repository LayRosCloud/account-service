using AccountService.Features.Transactions.CreateTransaction;
using AccountService.Features.Transactions.TransferBetweenAccounts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Features.Transactions;

[ApiController]
public class TransactionController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("/accounts/{accountId}/transactions")]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionCommand command, Guid accountId)
    {
        command.AccountId = accountId;
        var transaction = await _mediator.Send(command);
        return Ok(transaction);
    }

    [HttpPost("/accounts/{accountId}/to/{counterPartyAccountId}/transactions")]
    public async Task<IActionResult> TransferBetweenAccounts([FromBody] TransferBetweenAccountsCommand command, Guid accountId, Guid counterPartyAccountId)
    {
        command.AccountId = accountId;
        command.CounterPartyAccountId = counterPartyAccountId;
        var transaction = await _mediator.Send(command);
        return Ok(transaction);
    }
}