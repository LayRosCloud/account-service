using AccountService.Features.Transactions.CreateTransaction;
using AccountService.Features.Transactions.Dto;
using AccountService.Features.Transactions.FindByAccountIdTransactions;
using AccountService.Features.Transactions.TransferBetweenAccounts;
using AccountService.Utils.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Features.Transactions;

[ApiController]
[Route("/transactions")]
[Produces("application/json")]
public class TransactionController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Finds transactions
    /// </summary>
    /// <remarks>
    /// Finds transactions by account id
    /// </remarks>
    /// <param name="accountId">account id</param>
    /// <response code="200">The transaction was finds</response>
    /// <response code="400">Object data is invalid</response>
    /// <response code="404">Object is not found</response>
    /// <response code="401">Unauthorized</response>
    [HttpGet("/accounts/{accountId}/transactions")]
    [Authorize]
    [ProducesResponseType(typeof(MbResponse<List<TransactionFullDto>>), 200)]
    [ProducesResponseType(typeof(MbError), 400)]
    [ProducesResponseType(typeof(MbError), 404)]
    public async Task<IActionResult> FindByAccountId(Guid accountId)
    {
        var command = new FindByAccountIdTransactionsQuery(accountId);
        var transactions = await _mediator.Send(command);
        var result = ResultGenerator.Ok(transactions);
        return Ok(result);
    }

    /// <summary>
    /// Create transaction
    /// </summary>
    /// <remarks>
    /// Create transaction
    /// </remarks>
    /// <response code="201">The transaction was created</response>
    /// <response code="400">Object data is invalid</response>
    /// <response code="404">Object is not found</response>
    /// <response code="401">Unauthorized</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(MbResponse<TransactionFullDto>), 201)]
    [ProducesResponseType(typeof(MbError), 400)]
    [ProducesResponseType(typeof(MbError), 404)]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionCommand command)
    {
        var transaction = await _mediator.Send(command);
        var result = ResultGenerator.Create(transaction);
        return new ObjectResult(result) { StatusCode = 201 };
    }

    /// <summary>
    /// Creates a new transfer transaction
    /// </summary>
    /// <remarks>
    /// Creates a transfer transaction for a specific accounts for a specific amount
    /// </remarks>
    /// <response code="201">The transfer transaction  was created</response>
    /// <response code="400">Object data is invalid</response>
    /// <response code="404">Object is not found</response>
    /// <response code="401">Unauthorized</response>
    [HttpPost("transfer")]
    [Authorize]
    [ProducesResponseType(typeof(MbResponse<TransactionFullDto>), 201)]
    [ProducesResponseType(typeof(MbError), 400)]
    [ProducesResponseType(typeof(MbError), 404)]
    public async Task<IActionResult> TransferBetweenAccounts([FromBody] TransferBetweenAccountsCommand command)
    {
        command.Type = TransactionType.Credit;
        var transaction = await _mediator.Send(command);
        var result = ResultGenerator.Create(transaction);
        return new ObjectResult(result) { StatusCode = 201 };
    }
}