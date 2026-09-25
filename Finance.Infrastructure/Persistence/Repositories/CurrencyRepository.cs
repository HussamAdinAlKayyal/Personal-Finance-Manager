using Finance.Application.Abstractions;
using Finance.Application.Responses;
using Microsoft.EntityFrameworkCore;

namespace Finance.Infrastructure.Persistence.Repositories;

public class CurrencyRepository(AppDbContext context) : ICurrencyRepository
{
    private readonly AppDbContext context = context;

    public async Task<IEnumerable<CurrencyResponse>> GetAllAsync()
    {
        return await context.Currencies.AsNoTracking()
            .Select(c => new CurrencyResponse(c.Id, c.Code, c.Name))
            .ToListAsync();
    }

    public Task<string?> GetCodeAsync(int currencyId)
    {
        return context.Currencies.Where(c => c.Id == currencyId)
            .Select(c => c.Code)
            .FirstOrDefaultAsync();
    }

    public async Task<CurrencyResponse?> GetAsync(int currencyId)
    {
        return await context.Currencies.Where(c => c.Id == currencyId)
            .Select(c => new CurrencyResponse(c.Id, c.Code, c.Name))
            .FirstOrDefaultAsync();
    }

    public async Task<string?> GetNameAsync(int currencyId)
    {
        return await context.Currencies.Where(c => c.Id == currencyId)
            .Select(c => c.Name)
            .FirstOrDefaultAsync();
    }

    public Task<bool> ExistsAsync(int currencyId)
    {
        return context.Currencies.AnyAsync(c => c.Id == currencyId);
    }
}
