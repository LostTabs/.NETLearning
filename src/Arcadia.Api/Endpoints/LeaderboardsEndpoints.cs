using Arcadia.Api.Contracts;
using Arcadia.Api.Options;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;

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

    private static Results<Ok<LeaderboardsResponse>, BadRequest<string>> GetGlobalLeaderboards(int? limit, IOptions<LeaderboardOptions> options)
    {
        var verifiedLimit = limit ?? options.Value.DefaultLimit;
        
        if (limit > options.Value.MaxLimit)
        {
            return TypedResults.BadRequest("Limit exceeded");
        }
        
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

    private static Results<Ok<LeaderboardsResponse>, BadRequest<string>> GetAroundMeLeaderboards(int? range, IOptions<LeaderboardOptions> options)
    {
        if (range > options.Value.MaxRange)
        {
            return TypedResults.BadRequest("range exceeded");
        }
        
        var verifiedRange = range ?? options.Value.DefaultRange;
        
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