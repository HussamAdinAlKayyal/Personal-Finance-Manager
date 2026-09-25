using Finance.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Finance.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<TotalCalculatorService>();
        services.AddScoped<CategoryService>();
        services.AddScoped<FundService>();

        return services;
    }
}
