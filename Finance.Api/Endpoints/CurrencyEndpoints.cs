using Finance.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Finance.Api.Endpoints;

public static class CurrencyEndpoints
{
    public static void MapCurrencyEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("currencies");

        group.MapGet("{id}", async ([FromRoute] int id, [FromServices] ICurrencyRepository repository) =>
        {
            return TypedResults.Ok(await repository.GetAsync(id));
        });

        group.MapGet("/", async ([FromServices] ICurrencyRepository repository) =>
        {
            return TypedResults.Ok(await repository.GetAllAsync());
        });

        group.MapGet("convert", async ([FromServices] ICurrencyService service, [FromQuery] decimal amount, [FromQuery] DateOnly? date, [FromQuery] int? fromId, [FromQuery] int? toId) =>
        {
            if (fromId.HasValue && toId.HasValue)
            {
                if (date.HasValue)
                {
                    return TypedResults.Ok(await service.ConvertAsync(date.Value, amount, fromId.Value, toId.Value));
                }
                return TypedResults.Ok(await service.ConvertAsync(amount, fromId.Value, toId.Value));
            }
            throw new ArgumentException("Must have the id of the from and to currency.");
        });
    }
}
