using AccountService.Features.Transactions.CreateTransaction;
using AccountService.Features.Transactions.Dto;
using AccountService.Features.Transactions.FindByAccountIdTransactions;
using AccountService.Features.Transactions.TransferBetweenAccounts;
using AccountService.Utils.Middleware;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AccountService.Features.Transactions;

[ApiController]
[Route("/transactions")]
[SwaggerTag("transactions of accounts")]
public class TransactionController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/accounts/{accountId}/transactions")]
    [SwaggerOperation(
        Summary = "Finds transactions",
        Description = "Finds transactions by account id",
        OperationId = "FindByAccountId",
        Tags = new[] { "Transaction" }
    )]
    [SwaggerResponse(200, "The transaction was created", typeof(TransactionFullDto))]
    [SwaggerResponse(400, "Object data is invalid", typeof(ExceptionDto))]
    [SwaggerResponse(404, "Object is not found", typeof(ExceptionDto))]
    public async Task<IActionResult> FindByAccountId(Guid accountId)
    {
        var command = new FindByAccountIdTransactionsQuery(accountId);
        var transaction = await _mediator.Send(command);
        return Ok(transaction);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Creates a new transaction",
        Description = "Creates a transaction for a specific account for a specific amount",
        OperationId = "CreateTransaction",
        Tags = new[] { "Transaction" }
    )]
    [SwaggerResponse(200, "The transaction was created", typeof(TransactionFullDto))]
    [SwaggerResponse(400, "Object data is invalid", typeof(ExceptionDto))]
    [SwaggerResponse(404, "Object is not found", typeof(ExceptionDto))]
    public async Task<IActionResult> CreateTransaction(
        [FromBody] [SwaggerRequestBody("body for create transaction", Required = true)]
        CreateTransactionCommand command)
    {
        var transaction = await _mediator.Send(command);
        return Ok(transaction);
    }

    [HttpPost("transfer")]
    [SwaggerOperation(
        Summary = "Creates a new transfer transaction",
        Description = "Creates a transfer transaction for a specific accounts for a specific amount",
        OperationId = "CreateTransferTransaction",
        Tags = new[] { "Transaction" }
    )]
    [SwaggerResponse(200, "The transfer transaction  was created", typeof(TransactionFullDto))]
    [SwaggerResponse(400, "Object data is invalid", typeof(ExceptionDto))]
    [SwaggerResponse(404, "Object is not found", typeof(ExceptionDto))]
    public async Task<IActionResult> TransferBetweenAccounts(
        [FromBody] [SwaggerRequestBody("body for create transfer transaction", Required = true)]
        TransferBetweenAccountsCommand command)
    {
        command.Type = TransactionType.Credit;
        var transaction = await _mediator.Send(command);
        return Ok(transaction);
    }
}