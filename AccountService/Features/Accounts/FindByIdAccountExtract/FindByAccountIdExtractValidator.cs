using FluentValidation;

namespace AccountService.Features.Accounts.FindByIdAccountExtract;

public class FindByAccountIdExtractValidator : AbstractValidator<FindByIdAccountExtractQuery>
{
    public FindByAccountIdExtractValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Field 'accountId' is empty");
        RuleFor(x => x.DateStart)
            .LessThan(x => x.DateEnd).WithMessage("Date start greater date end");
    }
}