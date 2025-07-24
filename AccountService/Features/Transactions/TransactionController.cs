using AccountService.Features.Transactions.CreateTransaction;
using AccountService.Features.Transactions.Dto;
using AccountService.Features.Transactions.TransferBetweenAccounts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation(
        Summary = "Creates a new transaction",
        Description = "Creates a transaction for a specific account for a specific amount",
        OperationId = "CreateTransaction",
        Tags = new[] { "Transaction" }
    )]
    [SwaggerResponse(200, "The transaction was created", typeof(TransactionFullDto))]
    [SwaggerResponse(400, "Object data is invalid")]
    [SwaggerResponse(404, "Object is not found")]
    public async Task<IActionResult> CreateTransaction([FromBody, SwaggerRequestBody("body for create transaction", Required = true)] CreateTransactionCommand command, 
        [SwaggerParameter("account id", Required = true)] Guid accountId)
    {
        command.AccountId = accountId;
        var transaction = await _mediator.Send(command);
        return Ok(transaction);
    }

    [HttpPost("/accounts/{accountId}/to/{counterPartyAccountId}/transactions")]
    [SwaggerOperation(
        Summary = "Creates a new transfer transaction",
        Description = "Creates a transfer transaction for a specific accounts for a specific amount",
        OperationId = "CreateTransferTransaction",
        Tags = new[] { "Transaction" }
    )]
    [SwaggerResponse(200, "The transfer transaction  was created", typeof(TransactionFullDto))]
    [SwaggerResponse(400, "Object data is invalid")]
    [SwaggerResponse(404, "Object is not found")]
    public async Task<IActionResult> TransferBetweenAccounts([FromBody, SwaggerRequestBody("body for create transfer transaction", Required = true)] TransferBetweenAccountsCommand command, 
        [SwaggerParameter("account (from) id", Required = true)] Guid accountId,
        [SwaggerParameter("account (to) id", Required = true)] Guid counterPartyAccountId)
    {
        command.AccountId = accountId;
        command.CounterPartyAccountId = counterPartyAccountId;
        var transaction = await _mediator.Send(command);
        return Ok(transaction);
    }
}