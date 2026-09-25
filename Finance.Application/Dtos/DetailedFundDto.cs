using Finance.Domain.Enums;

namespace Finance.Application.Dtos;

public record DetailedFundDto(
    int Id,
    decimal Amount,
    int CurrencyId,
    int CategoryId,
    DateOnly Date,
    string Description,
    FundType FundType,
    decimal ExchangeRateInUsd);
