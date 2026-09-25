namespace Finance.Application.Responses;

public record FinancialInformationResponse(
    decimal Amount,
    decimal ExchangeRateInUsd);
