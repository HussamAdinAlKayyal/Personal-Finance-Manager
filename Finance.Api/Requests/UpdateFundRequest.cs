namespace Finance.Api.Requests;

public record UpdateFundRequest(
    decimal Amount,
    int? CurrencyId,
    DateOnly? Date,
    string? Description,
    int? CategoryId,
    string? FundType);
