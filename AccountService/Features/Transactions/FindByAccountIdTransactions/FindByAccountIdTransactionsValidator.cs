using FluentValidation;

namespace AccountService.Features.Transactions.FindByAccountIdTransactions;

public class FindByAccountIdTransactionsValidator : AbstractValidator<FindByAccountIdTransactionsQuery>
{
    public FindByAccountIdTransactionsValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Account is empty");
    }
}