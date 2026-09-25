namespace Finance.Api.Responses;

public record TotalCalculationResponse(
    decimal Total,
    string CurrencyCode,
    string CurrencyName);
