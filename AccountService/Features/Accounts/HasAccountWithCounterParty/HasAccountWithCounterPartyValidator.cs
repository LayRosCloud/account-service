using FluentValidation;

namespace AccountService.Features.Accounts.HasAccountWithCounterParty;

public class HasAccountWithCounterPartyValidator : AbstractValidator<HasAccountWithCounterPartyCommand>
{
    public HasAccountWithCounterPartyValidator()
    {
        RuleFor(account => account.AccountId)
            .NotEmpty().WithMessage("Field 'accountId' is empty");
        RuleFor(account => account.OwnerId)
            .NotEmpty().WithMessage("Field 'ownerId' is empty");
    }
}