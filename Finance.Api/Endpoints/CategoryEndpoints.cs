using Finance.Api.Extensions;
using Finance.Application.Commands;
using Finance.Application.Queries;
using Finance.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Finance.Api.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("categories").RequireAuthorization();

        group.MapGet("/", async ([FromServices] CategoryService service, ClaimsPrincipal user) =>
        {
            return TypedResults.Ok(await service.GetAsync(user.GetId()));
        });

        group.MapGet("{id:int}", async ([FromRoute] int id, [FromServices] CategoryService service, ClaimsPrincipal user) =>
        {
            return TypedResults.Ok(await service.GetAsync(new CategoryQuery(id, user.GetId())));
        });

        group.MapPost("/", async ([FromQuery, FromBody] string name, [FromServices] CategoryService service, ClaimsPrincipal user) =>
        {
            await service.CreateAsync(new CreateCategoryCommand(name, user.GetId()));
            return TypedResults.Created();
        });

        group.MapPut("{id:int}", async ([FromRoute] int id, [FromQuery, FromBody] string name, [FromServices] CategoryService service, ClaimsPrincipal user) =>
        {
            await service.UpdateAsync(new UpdateCategoryCommand(id, name, user.GetId()));
            return TypedResults.NoContent();
        });

        group.MapDelete("{id:int}", async ([FromRoute] int id, [FromServices] CategoryService service, ClaimsPrincipal user) =>
        {
            await service.DeleteAsync(new DeleteCategoryCommand(id, user.GetId()));
            return TypedResults.NoContent();
        });
    }
}
