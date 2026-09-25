using Finance.Application.Abstractions;

namespace Finance.Test.Application.Mocks;

internal class MockCurrencyService : ICurrencyService
{
    public Task<decimal> ConvertAsync(DateOnly date, decimal amount, string fromCurrencyCode, string toCurrencyCode)
    {
        return Task.FromResult(1m);
    }

    public Task<decimal> ConvertAsync(DateOnly date, decimal amount, int fromCurrencyId, int toCurrencyId)
    {
        return Task.FromResult(1m);
    }

    public Task<decimal> ConvertAsync(decimal amount, int fromCurrencyId, int toCurrencyId)
    {
        return Task.FromResult(1m);
    }

    public Task<decimal> ConvertAsync(decimal amount, string fromCurrencyCode, string toCurrencyCode)
    {
        return Task.FromResult(1m);
    }

    public Task<decimal> ConvertFromUsdAsync(decimal amount, int toCurrencyId)
    {
        return Task.FromResult(amount * toCurrencyId);
    }

    public Task<decimal> GetExchangeRateAsync(DateOnly date, int fromCurrencyId, int toCurrencyId)
    {
        return Task.FromResult(1m);
    }

    public Task<decimal> GetExchangeRateAsync(DateOnly date, string fromCurrencyCode, string toCurrencyCode)
    {
        return Task.FromResult(1m);
    }

    public Task<decimal> GetExchangeRateAsync(int fromCurrencyId, int toCurrencyId)
    {
        return Task.FromResult(1m);
    }

    public Task<decimal> GetExchangeRateAsync(string fromCurrencyCode, string toCurrencyCode)
    {
        return Task.FromResult(1m);
    }

    public Task<decimal> GetExchangeRateFromUsdAsync(DateOnly date, int toCurrencyId)
    {
        return Task.FromResult(1m);
    }

    public Task<decimal> GetExchangeRateFromUsdAsync(int toCurrencyId)
    {
        return Task.FromResult((decimal)toCurrencyId);
    }

    public Task<decimal> GetExchangeRateToUsdAsync(DateOnly date, int fromCurrencyId)
    {
        throw new NotImplementedException();
    }
}
