using MediatR;

namespace AccountService.Features.Currencies.VerifyCurrency;

public class VerifyCurrencyCommand : IRequest<bool>
{
    public VerifyCurrencyCommand(string code)
    {
        Code = code;
    }

    /// <summary>
    /// Code Currency
    /// </summary>
    public string Code { get; set; }
}