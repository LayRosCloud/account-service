using ISO._4217;
using MediatR;

namespace AccountService.Features.Currencies.VerifyCurrency;

public class VerifyCurrencyHandler : IRequestHandler<VerifyCurrencyCommand, bool>
{
    public Task<bool> Handle(VerifyCurrencyCommand request, CancellationToken cancellationToken)
    {
        var result = CurrencyCodesResolver.Codes.Any(currency => currency.Code == request.Code);
        return Task.FromResult(result);
    }
}