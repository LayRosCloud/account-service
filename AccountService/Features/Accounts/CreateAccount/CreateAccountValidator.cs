using FluentValidation;

namespace AccountService.Features.Accounts.CreateAccount;

public class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountValidator()
    {
        RuleFor(account => account.OwnerId)
            .NotEmpty().WithMessage("Field 'ownerId' is empty");
        RuleFor(account => account.Currency)
            .Must(code => ISO._4217.CurrencyCodesResolver.Codes.Any(c => c.Code == code))
            .WithMessage("Currency with incorrect code");
        RuleFor(account => account.Type)
            .NotNull().WithMessage("Field 'type' is nullable");
        RuleFor(account => account.Percent)
            .GreaterThanOrEqualTo(0).WithMessage("Field 'percent' is less 0");
        RuleFor(account => new { account.Balance, account.Type })
            .Must(x => (x.Type == AccountType.Deposit && x.Balance >= 0) || (x.Type == AccountType.Credit && x.Balance < 0))
            .WithMessage("Incorrect balance for account type.\nDeposit account balance greater or equals 0.\nCredit account balance less 0");
    }
}