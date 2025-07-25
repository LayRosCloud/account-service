using AccountService.Features.Currencies.FindAllCurrency;
using AccountService.Features.Currencies.VerifyCurrency;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AccountService.Features.Currencies;

[ApiController]
[Route("/currencies")]
[SwaggerTag("stub currencies")]
public class CurrencyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CurrencyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Find all currency",
        Description = "Find all currency",
        OperationId = "FindAllCurrencies",
        Tags = new[] { "Currency" }
    )]
    [SwaggerResponse(200, "The currency finds", typeof(List<string>))]
    public async Task<IActionResult> FindAllUsers()
    {
        var command = new FindAllCurrencyQuery();
        var currencies = await _mediator.Send(command);
        return Ok(currencies);
    }

    [HttpPost("{code}/verify")]
    [SwaggerOperation(
        Summary = "Verify currency",
        Description = "Check currency by code",
        OperationId = "VerifyCurrency",
        Tags = new[] { "Currency" }
    )]
    [SwaggerResponse(200, "The transaction was created", typeof(bool))]
    public async Task<IActionResult> Verify(string code)
    {
        var command = new VerifyCurrencyCommand(code);
        var currency = await _mediator.Send(command);
        return Ok(currency);
    }
}