using Finance.Application.Abstractions;
using Finance.Application.Queries;
using Finance.Domain.Enums;

namespace Finance.Application.Services;

public class TotalCalculatorService(IFundRepository repository, ICurrencyService currencyService)
{
    private readonly IFundRepository repository = repository;

    private readonly ICurrencyService currencyService = currencyService;

    public async Task<decimal> CalculateAsync(TotalQuery query)
    {
        if (string.IsNullOrWhiteSpace(query.FundType) || !Enum.TryParse(query.FundType, true, out FundType fundType))
        {

            var infos = await repository.GetFinancialFundInformationAsync(query.UserId);
            decimal result = 0.0m;
            foreach (var info in infos)
            {
                decimal total = info.Amount * info.ExchangeRateInUsd;
                if (info.FundType == FundType.Income)
                {
                    result += total;
                }
                else if (info.FundType == FundType.Expense)
                {
                    result -= total;
                }
            }
            return await currencyService.ConvertFromUsdAsync(result, query.CurrencyId);
        }
        return await currencyService.ConvertFromUsdAsync((await repository.GetFinancialFundInformationAsync(query.UserId, fundType)).Sum(x => x.Amount * x.ExchangeRateInUsd), query.CurrencyId);
    }
}
