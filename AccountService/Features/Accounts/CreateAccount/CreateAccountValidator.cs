using AccountService.Features.Currencies.VerifyCurrency;
using FluentValidation;
using MediatR;

namespace AccountService.Features.Accounts.CreateAccount;

public class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountValidator(IMediator mediator)
    {
        RuleFor(account => account.OwnerId)
            .NotEmpty().WithMessage("Field 'ownerId' is empty");

        RuleFor(account => account.Currency)
            .NotEmpty()
            .Must(code =>
            {
                var dto = new VerifyCurrencyCommand(code);
                return mediator.Send(dto).Result;
            })
            .WithMessage("Currency with incorrect code");

        RuleFor(account => account.Type)
            .NotNull().WithMessage("Field 'type' is nullable");

        RuleFor(account => account.Percent)
            .GreaterThan(0).WithMessage("Field 'percent' is less 0");

        RuleFor(account => new { account.Balance, account.Type, account.Percent })
            .Must(x => (x.Type == AccountType.Deposit && x.Balance >= 0) ||
                       (x.Type == AccountType.Credit && x.Balance < 0 && x.Percent != 0) ||
                       x.Type == AccountType.Checking)
            .WithMessage(
                "Incorrect balance for account type.\nDeposit account balance greater or equals 0.\nCredit account balance less 0");
    }
}