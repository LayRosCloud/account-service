using FluentValidation;

namespace AccountService.Features.Transactions.FindByAccountIdTransactions;

// ReSharper disable once UnusedMember.Global using ValidatorBehaviour
public class FindByAccountIdTransactionsValidator : AbstractValidator<FindByAccountIdTransactionsQuery>
{
    public FindByAccountIdTransactionsValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Account is empty");
    }
}