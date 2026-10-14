using Arcadia.Api.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Arcadia.Api.Endpoints;

public static class PlayerEndpoints
{
    public static IEndpointRouteBuilder MapPlayerEndpoints(this IEndpointRouteBuilder app)
    {
        var players = app.MapGroup("/players");
        players.MapPost("/", RegisterPlayer);
        
        var player = app.MapGroup("/players/{playerId:long}");
        player.MapGet("/profile", GetProfile);
        player.MapPatch("/profile", UpdateProfile);

        return app;
    }

    private static Results<Created<ProfileResponse>, BadRequest<string>> RegisterPlayer(PlayerRegister request)
    {
        if (string.IsNullOrWhiteSpace(request.userName))
        {
            return TypedResults.BadRequest("Username is required.");
        }

        long playerId = 123456;
        var response = new ProfileResponse(playerId, "Alice");
        return TypedResults.Created($"player/{playerId}/profile", response);
    }

    private static Results<Ok<ProfileResponse>, NotFound> GetProfile(long playerId)
    {
        // Hard-coded stub: pretend player 999 doesn't exist, so you can test the 404
        if (playerId == 999)
            return TypedResults.NotFound();

        return TypedResults.Ok(new ProfileResponse(playerId, "Alice"));
    }
    
    private static Results<Ok<ProfileResponse>, NotFound> UpdateProfile(long playerId)
    {
        if (playerId == 999)
        {
            return TypedResults.NotFound();
        }
        
        return TypedResults.Ok(new ProfileResponse(playerId, "Fred"));
    }
}