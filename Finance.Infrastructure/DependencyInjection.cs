using Finance.Application.Abstractions;
using Finance.Infrastructure.Identity;
using Finance.Infrastructure.Options;
using Finance.Infrastructure.Persistence;
using Finance.Infrastructure.Persistence.Repositories;
using Finance.Infrastructure.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finance.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<ICurrencyService, CurrencyService>();
        services.AddScoped<IFundRepository, FundRepository>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddDbContext<AppDbContext>(options =>
        {
            string? sqliteConnectionString = configuration.GetConnectionString("Sqlite");
            if (sqliteConnectionString != null)
            {
                options.UseSqlite(sqliteConnectionString);
            }
            else
            {
                options.UseInMemoryDatabase("FinanceDatabase");
            }
        });

        services.AddOptions<JwtOptions>().Bind(configuration.GetSection(JwtOptions.Section));

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>();

        return services;
    }
}
