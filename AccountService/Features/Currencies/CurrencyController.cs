using AccountService.Features.Currencies.FindAllCurrency;
using AccountService.Features.Currencies.VerifyCurrency;
using AccountService.Utils.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    /// <response code="401">Unauthorized</response>
    /// <response code="200">The currency finds</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(MbResponse<List<string>>), 200)]
    public async Task<IActionResult> FindAllCurrencies()
    {
        var command = new FindAllCurrencyQuery();
        var currencies = await _mediator.Send(command);
        var result = ResultGenerator.Ok(currencies);
        CausationHandler.ChangeCautionHeader(HttpContext, Guid.Parse("742f8901-cebf-4272-ac34-30e7339a22be"));
        return Ok(result);
    }

    /// <summary>
    /// Verify currency
    /// </summary>
    /// <remarks>
    /// Check currency by code
    /// </remarks>
    /// <response code="200">exist currency</response>
    /// <response code="401">Unauthorized</response>
    [HttpPost("{code}/verify")]
    [Authorize]
    [ProducesResponseType(typeof(MbResponse<bool>), 200)]
    public async Task<IActionResult> Verify(string code)
    {
        var command = new VerifyCurrencyCommand(code);
        var currency = await _mediator.Send(command);
        var result = ResultGenerator.Ok(currency);
        CausationHandler.ChangeCautionHeader(HttpContext, Guid.Parse("de2ec6dc-7a14-4fef-9f07-7accbbdf3677"));
        return Ok(result);
    }
}