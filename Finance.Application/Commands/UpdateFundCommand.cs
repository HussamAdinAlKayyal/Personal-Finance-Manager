using Finance.Application.Dtos;
using Finance.Domain.Enums;

namespace Finance.Application.Commands;

public record UpdateFundCommand(
    int FundId,
    decimal Amount,
    int? CurrencyId,
    DateOnly? Date,
    string? Description,
    string UserId,
    int? CategoryId,
    string? FundType)
{
    public UpdateFundDto ToUpdateFundDto(decimal? exchangeRateInUsd)
    {
        bool valid = Enum.TryParse(FundType, true, out FundType fundType);
        return new(
            FundId, 
            Amount, 
            CurrencyId, 
            Date, 
            Description, 
            UserId, 
            CategoryId, 
            exchangeRateInUsd,
            valid ? fundType : null);
    }
}