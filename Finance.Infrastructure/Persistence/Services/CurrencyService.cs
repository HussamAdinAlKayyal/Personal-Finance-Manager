using Finance.Application.Abstractions;
using Finance.Application.Exceptions;
using System.Net.Http.Json;

namespace Finance.Infrastructure.Persistence.Services;

public class CurrencyService(ICurrencyRepository currencyRepository) : ICurrencyService
{
    private record Transaction(decimal Rate);

    private readonly ICurrencyRepository currencyRepository = currencyRepository;

    private async Task<string> GetCurrencyCodeAsync(int currencyId)
    {
        return await currencyRepository.GetCodeAsync(currencyId) ??
                throw new EntityNotFoundException("Currency", currencyId);
    }

    private async Task<(string fromCurrencyCode, string toCurrencyCode)> GetCurrenciesCodes(int fromCurrencyId, int toCurrencyId)
    {
        string fromCurrencyCode = await GetCurrencyCodeAsync(fromCurrencyId),
               toCurrencyCode = await GetCurrencyCodeAsync(toCurrencyId);
        return (fromCurrencyCode, toCurrencyCode);
    }

    public async Task<decimal> ConvertAsync(DateOnly date, decimal amount, int fromCurrencyId, int toCurrencyId)
    {
        var (fromCurrencyCode, toCurrencyCode) = await GetCurrenciesCodes(fromCurrencyId, toCurrencyId);
        return await ConvertAsync(date, amount, fromCurrencyCode, toCurrencyCode);
    }

    public async Task<decimal> ConvertAsync(DateOnly date, decimal amount, string fromCurrencyCode, string toCurrencyCode)
    {
        return await GetExchangeRateAsync(date, fromCurrencyCode, toCurrencyCode) * amount;
    }

    public async Task<decimal> ConvertAsync(decimal amount, int fromCurrencyId, int toCurrencyId)
    {
        var (fromCurrencyCode, toCurrencyCode) = await GetCurrenciesCodes(fromCurrencyId, toCurrencyId);
        return await ConvertAsync(DateOnly.FromDateTime(DateTime.Now), amount, fromCurrencyCode, toCurrencyCode);
    }

    public async Task<decimal> ConvertAsync(decimal amount, string fromCurrencyCode, string toCurrencyCode)
    {
        return await GetExchangeRateAsync(fromCurrencyCode, toCurrencyCode) * amount;
    }

    public async Task<decimal> GetExchangeRateAsync(DateOnly date, string fromCurrencyCode, string toCurrencyCode)
    {
        using var client = new HttpClient()
        {
            BaseAddress = new Uri($"https://api.frankfurter.dev/v2/rate/{fromCurrencyCode.ToLower()}/{toCurrencyCode.ToLower()}?date={date:yyyy-MM-dd}"),
        };
        Transaction transaction = await client.GetFromJsonAsync<Transaction>("") ?? throw new Exception("Check your currencies' codes.");
        return transaction.Rate;
    }

    public async Task<decimal> GetExchangeRateAsync(DateOnly date, int fromCurrencyId, int toCurrencyId)
    {
        var (fromCurrencyCode, toCurrencyCode) = await GetCurrenciesCodes(fromCurrencyId, toCurrencyId);
        return await GetExchangeRateAsync(date, fromCurrencyCode, toCurrencyCode);
    }

    public async Task<decimal> GetExchangeRateAsync(int fromCurrencyId, int toCurrencyId)
    {
        var (fromCurrencyCode, toCurrencyCode) = await GetCurrenciesCodes(fromCurrencyId, toCurrencyId);
        return await GetExchangeRateAsync(fromCurrencyCode, toCurrencyCode);
    }

    public async Task<decimal> GetExchangeRateAsync(string fromCurrencyCode, string toCurrencyCode)
    {
        return await GetExchangeRateAsync(DateOnly.FromDateTime(DateTime.Now), fromCurrencyCode, toCurrencyCode);
    }

    public async Task<decimal> ConvertFromUsdAsync(decimal amount, int toCurrencyId)
    {
        string toCurrencyCode = await GetCurrencyCodeAsync(toCurrencyId);
        return await ConvertAsync(amount, ICurrencyService.UsdCurrencyCode, toCurrencyCode);
    }

    public async Task<decimal> GetExchangeRateFromUsdAsync(DateOnly date, int toCurrencyId)
    {
        string toCurrencyCode = await GetCurrencyCodeAsync(toCurrencyId);
        return await GetExchangeRateAsync(date, ICurrencyService.UsdCurrencyCode, toCurrencyCode);
    }

    public async Task<decimal> GetExchangeRateFromUsdAsync(int toCurrencyId)
    {
        return await GetExchangeRateFromUsdAsync(DateOnly.FromDateTime(DateTime.Now), toCurrencyId);
    }

    public async Task<decimal> GetExchangeRateToUsdAsync(DateOnly date, int fromCurrencyId)
    {
        string fromCurrencyCode = await GetCurrencyCodeAsync(fromCurrencyId);
        return await GetExchangeRateAsync(date, fromCurrencyCode, ICurrencyService.UsdCurrencyCode);
    }
}
