using Finance.Application.Abstractions;
using Finance.Application.Common;
using Finance.Domain.Entities;
using Finance.Domain.Enums;

namespace Finance.Application.Commands;

public record CreateFundCommand(
    decimal Amount,
    int CurrencyId,
    DateOnly Date,
    string? Description,
    string UserId,
    int CategoryId,
    string FundType)
{
    public async Task<Fund> AsFundAsync(ICurrencyService currencyService)
    {
        bool valid = Enum.TryParse(FundType, true, out FundType fundType);
        Fund fund = new(
            valid ? fundType : Domain.Enums.FundType.Income,
            Amount,
            CurrencyId,
            Date,
            await currencyService.GetExchangeRateToUsdAsync(Date, CurrencyId),
            Description,
            UserId,
            CategoryId);
        return fund;
    }
}