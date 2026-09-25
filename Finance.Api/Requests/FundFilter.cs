namespace Finance.Api.Requests;

public record FundFilter(
    int? CategoryId,
    int? CurrencyId,
    string? FundType);
