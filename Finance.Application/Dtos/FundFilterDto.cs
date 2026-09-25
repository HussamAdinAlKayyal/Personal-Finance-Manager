using Finance.Domain.Enums;

namespace Finance.Application.Dtos;

public record FundFilterDto(
    FundType? FundType,
    int? CurrencyId,
    int? CategoryId);
