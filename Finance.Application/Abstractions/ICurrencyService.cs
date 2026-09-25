namespace Finance.Application.Abstractions;

public interface ICurrencyService
{
    protected const string UsdCurrencyCode = "usd";
    Task<decimal> ConvertAsync(DateOnly date, decimal amount, string fromCurrencyCode, string toCurrencyCode);
    Task<decimal> ConvertAsync(DateOnly date, decimal amount, int fromCurrencyId, int toCurrencyId);
    Task<decimal> ConvertAsync(decimal amount, int fromCurrencyId, int toCurrencyId);
    Task<decimal> ConvertAsync(decimal amount, string fromCurrencyCode, string toCurrencyCode);
    Task<decimal> ConvertFromUsdAsync(decimal amount, int toCurrencyId);
    Task<decimal> GetExchangeRateAsync(DateOnly date, int fromCurrencyId, int toCurrencyId);
    Task<decimal> GetExchangeRateAsync(DateOnly date, string fromCurrencyCode, string toCurrencyCode);
    Task<decimal> GetExchangeRateFromUsdAsync(DateOnly date, int toCurrencyId);
    Task<decimal> GetExchangeRateToUsdAsync(DateOnly date, int fromCurrencyId);
    Task<decimal> GetExchangeRateFromUsdAsync(int toCurrencyId);
    Task<decimal> GetExchangeRateAsync(int fromCurrencyId, int toCurrencyId);
    Task<decimal> GetExchangeRateAsync(string fromCurrencyCode, string toCurrencyCode);
}
