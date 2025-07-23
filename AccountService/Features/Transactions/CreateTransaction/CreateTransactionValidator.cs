using FluentValidation;

namespace AccountService.Features.Transactions.CreateTransaction;

public class CreateTransactionValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionValidator()
    {
        RuleFor(transaction => transaction.AccountId)
            .NotEmpty().WithMessage("Field 'accountId' is empty");
        RuleFor(transaction => transaction.Description)
            .NotEmpty().WithMessage("Field 'description' is empty");
        RuleFor(transaction => transaction.Type)
            .NotNull().WithMessage("Field 'type' is null");
        RuleFor(transaction => transaction.AccountId)
            .NotEmpty().WithMessage("Field 'accountId' is null");
    }
}