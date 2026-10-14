using Arcadia.Api.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Arcadia.Api.Endpoints;

public static class ShopEndpoints
{
    public static IEndpointRouteBuilder MapShopEndpoints(this IEndpointRouteBuilder app)
    {
        var shop = app.MapGroup("/shop");
        shop.MapGet("/catalog", GetShopCatalog);

        return app;
    }

    private static Results<Ok<CatalogueResponse>, BadRequest> GetShopCatalog()
    {

        var catalog = new List<CatalogueProduct>
        {
            new(123, "xpot", 12),
            new(124, "xpto", 10)
        };
        return TypedResults.Ok(new CatalogueResponse(catalog));
    }
}