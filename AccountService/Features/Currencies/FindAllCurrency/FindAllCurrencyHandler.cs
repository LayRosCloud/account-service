using ISO._4217;
using MediatR;

namespace AccountService.Features.Currencies.FindAllCurrency;

// ReSharper disable once UnusedMember.Global using Mediator
public class FindAllCurrencyHandler : IRequestHandler<FindAllCurrencyQuery, List<string>>
{
    public Task<List<string>> Handle(FindAllCurrencyQuery request, CancellationToken cancellationToken)
    {
        var codes = CurrencyCodesResolver.Codes.Select(x => x.Code).ToList();
        return Task.FromResult(codes);
    }
}