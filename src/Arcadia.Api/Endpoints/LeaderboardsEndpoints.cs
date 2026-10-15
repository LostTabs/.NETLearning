using Arcadia.Api.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Arcadia.Api.Endpoints;

public static class LeaderboardsEndpoints
{
    public static IEndpointRouteBuilder MapLeaderboardEndpoints(this IEndpointRouteBuilder app)
    {
        var leaderboards = app.MapGroup("/leaderboards");
        leaderboards.MapGet("/global", GetGlobalLeaderboards);
        leaderboards.MapGet("/global/around-me", GetAroundMeLeaderboards);

        return app;
    }

    private static Ok<LeaderboardsResponse> GetGlobalLeaderboards(int? limit)
    {
        var verifiedLimit = limit ?? 50;
        
        var results = new List<PlayerEntry>
        {
            new(123456, 1, 100),
            new(123457, 2, 90),
        };
        
        if (results.Count > verifiedLimit)
        {
            results.RemoveRange(verifiedLimit, results.Count - verifiedLimit);
        }
        
        return TypedResults.Ok(new LeaderboardsResponse(results));
    }

    private static Results<Ok<LeaderboardsResponse>, BadRequest> GetAroundMeLeaderboards(int? range)
    {
        if (range < 0)
        {
            return TypedResults.BadRequest();
        }
        
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