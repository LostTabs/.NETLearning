using System.Diagnostics;
using Arcadia.Api.Contracts;
using Arcadia.Api.Players;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Arcadia.Api.Endpoints;

public static class PlayerEndpoints
{
    public static IEndpointRouteBuilder MapPlayerEndpoints(this IEndpointRouteBuilder app)
    {
        var players = app.MapGroup("/players");
        
        players.MapPost("/", RegisterPlayer);
        
        var player = players.MapGroup("/{playerId:long}");
        player.MapGet("/profile", GetPlayer);
        player.MapPatch("/profile", UpdateProfile);
        player.MapGet("/currencies", GetCurrencies);
        player.MapGet("/inventory", GetInventory);
        
        player.MapPost("/purchases", BuyItem);
        player.MapGet("/purchases", GetPurchaseHistory);
        
        return app;
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
        
        return TypedResults.Created($"/players/{playerId}/purchases/{response.PurchaseId}", response);
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
    
    private static async Task<Results<Ok<ProfileResponse>, NotFound>>  GetPlayer(long playerId, IPlayerRepository repo, CancellationToken ct)
    {
        var player = await repo.GetAsync(playerId, ct);
        return player is null ? TypedResults.NotFound() : TypedResults.Ok(new ProfileResponse(player.Id, player.Username));
    }

    private static async
        Task<Results<Created<ProfileResponse>, BadRequest<string>, Conflict<string>>>
        RegisterPlayer(CreatePlayerRequest request,
            IPlayerRepository repo,
            CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Username)) return TypedResults.BadRequest("Username is required");

        var (status, player) = await repo.CreateAsync(request.Username, request.Email, request.Country, ct);
        
        return status switch
        {
            CreatePlayerStatus.UsernameTaken => TypedResults.Conflict("Username is already taken"),
            CreatePlayerStatus.EmailTaken => TypedResults.Conflict("Email is already taken"),
            CreatePlayerStatus.Created => TypedResults.Created($"/players/{player!.Id}/profile", 
                                            new ProfileResponse(player.Id, player.Username)),
            _ => throw new UnreachableException()
        };
    }
}