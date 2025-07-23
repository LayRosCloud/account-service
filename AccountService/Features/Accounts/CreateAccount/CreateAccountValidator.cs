using FluentValidation;

namespace AccountService.Features.Accounts.CreateAccount;

public class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountValidator()
    {
        RuleFor(account => account.OwnerId)
            .NotEmpty();
        RuleFor(account => account.Currency)
            .Must(code => ISO._4217.CurrencyCodesResolver.Codes.Any(c => c.Code == code));
        RuleFor(account => account.Type)
            .NotNull();
        RuleFor(account => account.Percent)
            .GreaterThanOrEqualTo(0);
    }
}