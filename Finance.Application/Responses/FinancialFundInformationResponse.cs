using Finance.Domain.Enums;

namespace Finance.Application.Responses;

public record FinancialFundInformationResponse(
    decimal ExchangeRateInUsd,
    decimal Amount,
    FundType FundType);
