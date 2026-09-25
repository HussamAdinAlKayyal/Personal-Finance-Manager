using Finance.Api.Requests;
using FluentValidation;

namespace Finance.Api.Validators;

public class CreateFundRequestValidator : AbstractValidator<CreateFundRequest>
{
    public CreateFundRequestValidator()
    {
        RuleFor(r => r.Amount)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(r => r.CategoryId)
            .NotEmpty();

        RuleFor(r => r.CurrencyId)
            .NotEmpty();

        RuleFor(r => r.FundType)
            .NotEmpty()
            .Must(s => s.Equals("income", StringComparison.InvariantCultureIgnoreCase) || s.Equals("expense", StringComparison.InvariantCultureIgnoreCase));

        RuleFor(r => r.Date)
            .NotEmpty()
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));
    }
}
