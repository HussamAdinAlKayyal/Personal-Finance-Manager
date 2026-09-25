using Finance.Application.Responses;
using Finance.Domain.Enums;

namespace Finance.Application.Dtos;

public record FundDto(
    int Id,
    decimal Amount,
    string CurrencyCode,
    DateOnly Date,
    string Description,
    string CategoryName,
    FundType FundType)
{
    public FundResponse ToFundResponse()
    {
        return new(
            Id,
            Amount,
            CurrencyCode,
            Date,
            Description,
            CategoryName,
            FundType.ToString());
    }
}