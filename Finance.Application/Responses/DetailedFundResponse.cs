namespace Finance.Application.Responses;

public record DetailedFundResponse(
    int Id,
    string FundType,
    decimal Amount,
    string CurrencyName,
    string CurrencyCode,
    string CategoryName,
    DateOnly Date,
    string Description,
    decimal ExchangeRateInUsd);
