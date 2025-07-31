using FluentValidation;

namespace AccountService.Features.Accounts.DeleteAccount;

// ReSharper disable once UnusedMember.Global using ValidatorBehaviour
public class DeleteAccountValidator : AbstractValidator<DeleteAccountCommand>
{
    public DeleteAccountValidator()
    {
        RuleFor(account => account.AccountId)
            .NotEmpty().WithMessage("Field 'accountId' is empty");
    }
}