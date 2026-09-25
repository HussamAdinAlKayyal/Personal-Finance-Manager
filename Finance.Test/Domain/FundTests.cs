using Finance.Domain.Entities;
using Finance.Domain.Enums;

namespace Finance.Test.Domain;

public static class FundTests
{
    private static readonly Fund fund =
        new(fundType: FundType.Income,
            amount: decimal.MaxValue,
            currencyId: 0,
            date: new(),
            exchangeRateInUsd: decimal.MaxValue,
            description: null,
            userId: "",
            categoryId: 0);

    private static readonly DateOnly oldDate = new(2000, 1, 1);

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public static void SetAmount_NegativeOrZeroAmount_ShouldFail(int amount)
    {
        Assert.Throws<ArgumentException>(() =>
        {
            fund.SetAmount(amount);
        });
    }

    [Fact]
    public static void SetAmount_PositiveAmount_ShouldSucceed()
    {
        fund.SetAmount(1);
        Assert.Equal(1, fund.Amount);
    }

    [Fact]
    public static void SetDateAndExchangeRateInUsd_FutureDate_ShouldFail()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            fund.SetDateAndExchangeRateInUsd(DateOnly.MaxValue, 1);
        });
    }

    [Fact]
    public static void SetDateAndExchangeRateInUsd_OldDate_ShouldSucceed()
    {
        fund.SetDateAndExchangeRateInUsd(oldDate, 1);
        Assert.Equal(oldDate, fund.Date);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-12)]
    public static void SetDateAndExchangeRateInUsd_LessThanZeroExchangeRate_ShouldFail(decimal exchangeRateInUsd)
    {
        Assert.Throws<ArgumentException>(() =>
        {
            fund.SetDateAndExchangeRateInUsd(oldDate, exchangeRateInUsd);
        });
    }

    [Fact]
    public static void SetDateAndExchangeRateInUsd_GreaterThanZeroExchangeRate_ShouldSucceed()
    {
        fund.SetDateAndExchangeRateInUsd(oldDate, 1);
        Assert.Equal(1, fund.ExchangeRateInUsd);
    }
}
