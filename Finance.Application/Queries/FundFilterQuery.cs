using Finance.Application.Dtos;
using Finance.Domain.Enums;

namespace Finance.Application.Queries;

public record FundFilterQuery(
    string? FundType,
    int? CurrencyId,
    int? CategoryId)
{
    public FundFilterDto ToFundFilterDto()
    {
        if (string.IsNullOrWhiteSpace(FundType))
        {
            return GetFundFilterDto(null);
        }
        if (Enum.TryParse(FundType, true, out FundType fundType))
        {
            return GetFundFilterDto(fundType);
        }
        return GetFundFilterDto(Domain.Enums.FundType.Income);
    }

    private FundFilterDto GetFundFilterDto(FundType? fundType)
    {
        return new(fundType, CurrencyId, CategoryId);
    }
}