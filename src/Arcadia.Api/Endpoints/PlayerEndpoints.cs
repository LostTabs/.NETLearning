using Arcadia.Api.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Arcadia.Api.Endpoints;

public static class PlayerEndpoints
{
    public static IEndpointRouteBuilder MapPlayerEndpoints(this IEndpointRouteBuilder app)
    {
        var players = app.MapGroup("/players");
        players.MapPost("/", RegisterPlayer);
        
        var player = players.MapGroup("/{playerId:long}");
        player.MapGet("/profile", GetProfile);
        player.MapPatch("/profile", UpdateProfile);
        player.MapGet("/currencies", GetCurrencies);
        player.MapGet("/inventory", GetInventory);

        player.MapPost("/purchases", BuyItem);
        player.MapGet("/purchases", GetPurchaseHistory);
        
        return app;
    }

    private static Results<Created<ProfileResponse>, BadRequest<string>> RegisterPlayer(RegisterPlayerRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
        {
            return TypedResults.BadRequest("Username is required.");
        }

        long playerId = 123456;
        var response = new ProfileResponse(playerId, request.Username);
        return TypedResults.Created($"/players/{playerId}/profile", response);
    }

    private static Results<Ok<ProfileResponse>, NotFound> GetProfile(long playerId)
    {
        // Hard-coded stub: pretend player 999 doesn't exist, so you can test the 404
        if (playerId == 999)
            return TypedResults.NotFound();

        return TypedResults.Ok(new ProfileResponse(playerId, "Alice"));
    }
    
    private static Results<Ok<ProfileResponse>, NotFound, Conflict> UpdateProfile(long playerId, UpdateProfileRequest request)
    {
        if (playerId == 999)
        {
            return TypedResults.NotFound();
        }

        if (request.Username == "Taken")
        {
            return TypedResults.Conflict();
        }
        
        return TypedResults.Ok(new ProfileResponse(playerId, request.Username));
    }
    
    
    private static Results<Ok<InventoryResponse>, NotFound> GetInventory(long playerId)
    {
        if (playerId == 999)
        {
            return TypedResults.NotFound();
        }
        
        var items = new List<Item>()
        {
            new Item(133, 1),
            new Item(134, 1),
        };
        return TypedResults.Ok(new InventoryResponse(items));
    }

    private static Results<Ok<CurrenciesResponse>, NotFound> GetCurrencies(long playerId)
    {
        if (playerId == 999)
        {
            return TypedResults.NotFound();
        }
        
        var items = new List<Currency>()
        {
            new Currency(123, 1),
            new Currency(124, 1),
        };
        return TypedResults.Ok(new CurrenciesResponse(items));
    }

    private static Results<Created<PurchaseResponse>, Conflict, NotFound> BuyItem(long playerId, PurchaseRequest request)
    {
        if (playerId == 999 || request.ItemId == 999)
        {
            return TypedResults.NotFound();
        }

        if (request.ItemId == 998)
        {
            return TypedResults.Conflict();
        }

        var response = new PurchaseResponse(123456, 123, 10, 1);
        
        return TypedResults.Created($"/players/{playerId}/purchase/{response.PurchaseId}", response);
    }

    private static Results<Ok<PurchaseHistoryResponse>, NotFound> GetPurchaseHistory(long playerId)
    {
        if (playerId == 999)
        {
            return TypedResults.NotFound();
        }

        var collection = new List<Purchase>
        {
            new (123, 1, 10, new DateTime(2020, 01, 01, 0, 0, 0, DateTimeKind.Utc)),
            new (124, 2, 12, new DateTime(2020, 01, 03, 0, 0, 0, DateTimeKind.Utc)),
        };
        return TypedResults.Ok(new PurchaseHistoryResponse(collection));
        
    }

}