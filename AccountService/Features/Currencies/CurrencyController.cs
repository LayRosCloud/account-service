using AccountService.Features.Currencies.FindAllCurrency;
using AccountService.Features.Currencies.VerifyCurrency;
using AccountService.Utils.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Features.Currencies;

[ApiController]
[Route("/currencies")]
[Produces("application/json")]
public class CurrencyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CurrencyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Find all currency
    /// </summary>
    /// <remarks>
    /// Find all currency
    /// </remarks>
    /// <response code="200">The currency finds</response>
    [HttpGet]
    [ProducesResponseType(typeof(MbResponse<List<string>>), 200)]
    public async Task<IActionResult> FindAllUsers()
    {
        var command = new FindAllCurrencyQuery();
        var currencies = await _mediator.Send(command);
        var result = ResultGenerator.Ok(currencies);
        return Ok(result);
    }

    /// <summary>
    /// Verify currency
    /// </summary>
    /// <remarks>
    /// Check currency by code
    /// </remarks>
    /// <response code="200">exist currency</response>
    [HttpPost("{code}/verify")]
    [ProducesResponseType(typeof(MbResponse<bool>), 200)]
    public async Task<IActionResult> Verify(string code)
    {
        var command = new VerifyCurrencyCommand(code);
        var currency = await _mediator.Send(command);
        var result = ResultGenerator.Ok(currency);
        return Ok(result);
    }
}