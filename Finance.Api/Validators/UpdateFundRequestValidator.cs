using Finance.Api.Requests;
using FluentValidation;

namespace Finance.Api.Validators;

public class UpdateFundRequestValidator : AbstractValidator<UpdateFundRequest>
{
    public UpdateFundRequestValidator()
    {
        RuleFor(r => r.Amount)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(r => r.Date)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));
    }
}
