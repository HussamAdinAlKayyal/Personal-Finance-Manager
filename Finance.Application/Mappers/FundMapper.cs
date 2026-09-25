using Finance.Application.Dtos;
using Finance.Application.Responses;
using Finance.Domain.Entities;

namespace Finance.Application.Mappers;

public static class FundMapper
{
    public static FinancialFundInformationResponse ToFinancialFundInformationResponse(this Fund fund)
    {
        return new(fund.ExchangeRateInUsd, fund.Amount, fund.FundType);
    }

    public static FinancialInformationResponse ToFinancialInformationResponse(this Fund fund)
    {
        return new(fund.Amount, fund.ExchangeRateInUsd);
    }

    public static DetailedFundDto ToDetailedFundDto(this Fund fund)
    {
        return new(
            fund.Id,
            fund.Amount,
            fund.CurrencyId,
            fund.CategoryId,
            fund.Date,
            fund.Description ?? "",
            fund.FundType,
            fund.ExchangeRateInUsd);
    }

    public static FundDto ToFundDto(this Fund fund, string code, string categoryName)
    {
        return new(
                fund.Id,
                fund.Amount,
                code,
                fund.Date,
                fund.Description ?? "",
                categoryName,
                fund.FundType);
    }
}
