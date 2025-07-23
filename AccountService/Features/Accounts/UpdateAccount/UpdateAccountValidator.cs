using FluentValidation;

namespace AccountService.Features.Accounts.UpdateAccount;
public class UpdateAccountValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountValidator()
    {
        RuleFor(account => account.Id)
            .NotEmpty().WithMessage("Field 'id' is empty");
        RuleFor(account => account.Percent)
            .GreaterThanOrEqualTo(0).WithMessage("Field 'percent' is less 0");
    }
}
