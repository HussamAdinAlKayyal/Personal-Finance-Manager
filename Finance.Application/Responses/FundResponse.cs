namespace Finance.Application.Responses;

public record FundResponse(
    int Id,
    decimal Amount,
    string CurrencyCode,
    DateOnly Date,
    string Description,
    string CategoryName,
    string FundType);
