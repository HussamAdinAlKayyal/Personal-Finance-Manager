using Finance.Domain.Enums;

namespace Finance.Application.Dtos;

public record UpdateFundDto(
    int FundId,
    decimal Amount,
    int? CurrencyId,
    DateOnly? Date,
    string? Description,
    string UserId,
    int? CategoryId,
    decimal? ExchangeRateInUsd,
    FundType? FundType);