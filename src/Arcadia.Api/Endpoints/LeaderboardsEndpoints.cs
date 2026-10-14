using Arcadia.Api.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Arcadia.Api.Endpoints;

public static class LeaderboardsEndpoints
{
    public static IEndpointRouteBuilder MapLeaderboardEndpoints(this IEndpointRouteBuilder app)
    {
        var leaderboards = app.MapGroup("/leaderboards");
        leaderboards.MapGet("/global?limit=value", GetGlobalLeaderboards);
        leaderboards.MapGet("/global/around-me?limit=value", GetAroundMeLeaderboards);

        return app;
    }

    private static Results<Ok<LeaderboardsResponse>, NotFound> GetGlobalLeaderboards(int? range)
    {
        var verifiedRange = range ?? 50;
        
        var results = new List<PlayerEntry>
        {
            new(123456, 1, 100),
            new(123457, 2, 90),
        };
        
        if (results.Count > verifiedRange)
        {
            results.RemoveRange(verifiedRange, results.Count - verifiedRange);
        }
        
        return TypedResults.Ok(new LeaderboardsResponse(results));
    }

    private static Results<Ok<LeaderboardsResponse>, NotFound> GetAroundMeLeaderboards(int? range)
    {
        var verifiedRange = range ?? 50;
        
        var results = new List<PlayerEntry>
        {
            new(123456, 1, 100),
            new(123457, 2, 90),
        };

        if (results.Count > verifiedRange)
        {
            results.RemoveRange(verifiedRange, results.Count - verifiedRange);
        }
        
        return TypedResults.Ok(new LeaderboardsResponse(results));
    }
}