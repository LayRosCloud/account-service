using FluentValidation;

namespace AccountService.Features.Accounts.DeleteAccount;

public class DeleteAccountValidator : AbstractValidator<DeleteAccountCommand>
{
    public DeleteAccountValidator()
    {
        RuleFor(account => account.AccountId)
            .NotEmpty().WithMessage("Field 'accountId' is empty");
    }
}