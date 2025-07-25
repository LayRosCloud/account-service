using FluentValidation;

namespace AccountService.Features.Accounts.UpdatePercentAccount;

public class UpdateAccountValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountValidator()
    {
        RuleFor(account => account.Id)
            .NotEmpty().WithMessage("Field 'id' is empty");
        RuleFor(account => account.Percent)
            .GreaterThan(0).WithMessage("Field 'percent' is less 0");
    }
}