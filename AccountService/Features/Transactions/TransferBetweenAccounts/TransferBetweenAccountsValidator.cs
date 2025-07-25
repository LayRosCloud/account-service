using FluentValidation;

namespace AccountService.Features.Transactions.TransferBetweenAccounts;

public class TransferBetweenAccountsValidator : AbstractValidator<TransferBetweenAccountsCommand>
{
    public TransferBetweenAccountsValidator()
    {
        RuleFor(transaction => transaction.Sum)
            .NotEmpty().WithMessage("Field 'sum' is empty")
            .GreaterThan(0).WithMessage("Field 'sum' is less or equals 0");

        RuleFor(transaction => transaction.AccountId)
            .NotEmpty().WithMessage("Field 'accountId' is empty")
            .NotEqual(transaction => transaction.CounterPartyAccountId)
            .WithMessage("Field 'counterpartyId' equals 'accountId'");

        RuleFor(transaction => transaction.CounterPartyAccountId)
            .NotEmpty().WithMessage("Field 'counterpartyId' is empty")
            .NotEqual(transaction => transaction.AccountId)
            .WithMessage("Field 'counterpartyId' equals 'accountId'");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Field 'description' is empty");
    }
}