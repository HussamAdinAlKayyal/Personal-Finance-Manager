using Finance.Domain.Enums;
using Finance.Domain.ValueObjects;

namespace Finance.Domain.Entities;

public class Fund
{
    public int Id { get; private set; }
    public FundType FundType { get; private set; }
    public Money Money { get; private set; }
    public DateOnly Date { get; private set; }
    public string? Description { get; set; }
    public decimal ExchangeRateInUsd { get; private set; }
    public string UserId { get; private set; }
    public int CategoryId { get; private set; }
    public decimal Amount => Money.Amount;
    public int CurrencyId => Money.CurrencyId;
    public bool IsIncome => FundType == FundType.Income;
    public bool IsExpense => FundType == FundType.Expense;

    protected Fund()
    {
        Money = new(1, 0);
        UserId = "";
    }

    public Fund(FundType fundType, decimal amount, int currencyId, DateOnly date, decimal exchangeRateInUsd, string? description, string userId, int categoryId)
    {
        SetDate(date);
        SetExchangeRateInUsd(exchangeRateInUsd);
        Money = new(amount, currencyId);
        Description = description;
        UserId = userId;
        CategoryId = categoryId;
        FundType = fundType;
    }

    public void SetAmount(decimal? amount)
    {
        if (amount.HasValue)
        {
            Money.SetAmount(amount.Value);
        }
    }

    public void SetCurrencyAndExchangeRateInUsd(int? currencyId, decimal? exchangeRateInUsd)
    {
        if (currencyId.HasValue)
        {
            SetExchangeRateInUsd(exchangeRateInUsd);
            Money.CurrencyId = currencyId.Value;
        }
    }

    public void SetCategory(int? categoryId)
    {
        if (categoryId.HasValue)
        {
            CategoryId = categoryId.Value;
        }
    }

    private void SetDate(DateOnly? date)
    {
        if (!date.HasValue)
        {
            return;
        }
        if (date.Value > DateOnly.FromDateTime(DateTime.Now))
        {
            throw new ArgumentException("Date cannot be in the future.", nameof(date));
        }
        Date = date.Value;
    }

    public void SetDateAndExchangeRateInUsd(DateOnly? date, decimal? exchangeRateInUsd)
    {
        SetDate(date);
        SetExchangeRateInUsd(exchangeRateInUsd);
    }

    public void SetDateAndCurrencyAndExchangeRateInUsd(DateOnly? date, int? currencyId, decimal? exchangeRateInUsd)
    {
        SetDate(date);
        SetCurrencyAndExchangeRateInUsd(currencyId, exchangeRateInUsd);
    }

    private void SetExchangeRateInUsd(decimal? exchangeRateInUsd)
    {
        if (!exchangeRateInUsd.HasValue || exchangeRateInUsd <= 0)
        {
            throw new ArgumentException("Exchange rate in USD must be greater than zero.", nameof(exchangeRateInUsd));
        }
        ExchangeRateInUsd = exchangeRateInUsd.Value;
    }

    public void SetFundType(FundType? fundType)
    {
        if (fundType.HasValue)
        {
            FundType = fundType.Value;
        }
    }
}
