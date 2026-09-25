using Finance.Application.Responses;

namespace Finance.Application.Abstractions;

public interface ICurrencyRepository
{
    Task<IEnumerable<CurrencyResponse>> GetAllAsync();
    Task<CurrencyResponse?> GetAsync(int currencyId);
    Task<string?> GetCodeAsync(int currencyId);
    Task<string?> GetNameAsync(int currencyId);
    Task<bool> ExistsAsync(int currencyId);
}
