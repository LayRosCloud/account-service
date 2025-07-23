using FluentValidation;

namespace AccountService.Features.Accounts.FindByIdAccount;
public class FindByAccountIdValidator : AbstractValidator<FindByIdAccountQuery>
{
    public FindByAccountIdValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Field 'accountId' is empty");
    }
}
