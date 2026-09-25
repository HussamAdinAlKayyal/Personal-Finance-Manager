using Finance.Domain.Entities;
using Finance.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Finance.Api.Extensions;

public static class WebApplicationExtensions
{
    private sealed record CurrencySeed(string Code, string Name);

    public static void MigrateDb(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (!context.Database.IsInMemory())
        {
            context.Database.Migrate();
        }
    }

    public static void SeedDb(this WebApplication app, IConfiguration configuration)
    {
        //string? configuredPath = configuration["CurrenciesDataLocation"];
        //if (string.IsNullOrWhiteSpace(configuredPath))
        //{
        //    throw new InvalidOperationException("CurrenciesDataLocation is not configured.");
        //}

        //string path = Path.IsPathRooted(configuredPath)
        //    ? configuredPath
        //    : Path.Combine(app.Environment.ContentRootPath, configuredPath);

        //if (!File.Exists(path))
        //{
        //    throw new FileNotFoundException("Unable to find the currencies data file.", path);
        //}

        //string json = File.ReadAllText(path);
        //JsonSerializerOptions options = JsonSerializerOptions.Web;
        //List<Currency>? data = [.. JsonSerializer.Deserialize<IEnumerable<Currency>>(json, options) ?? []];

        //if (data is null || data.Count == 0)
        //{
        //    throw new InvalidOperationException("No currency to insert to the table.");
        //}

        //for (int i = 1; i < data.Count; i++)
        //{
        //    data[i].Id = i;
        //}

        //using IServiceScope scope = app.Services.CreateScope();
        //AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //context.Currencies.UpdateRange(data);
        //context.SaveChanges();
    }
}
