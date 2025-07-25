using MediatR;

namespace AccountService.Features.Currencies.FindAllCurrency;

public class FindAllCurrencyQuery : IRequest<List<string>>
{
}