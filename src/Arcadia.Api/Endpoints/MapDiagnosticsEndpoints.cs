using Arcadia.Api.Diagnostics;
using Arcadia.Api.Infrastructure;
using Arcadia.Api.Leaderboards;
using Dapper;
using Npgsql;

namespace Arcadia.Api.Endpoints;

public static class DiagnosticsEndpoints
{
    public static IEndpointRouteBuilder MapDiagnosticsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/di/lifetimes", LifeTimes);
        app.MapGet("/debug/correlation", Correlation);
        app.MapGet("/leaderboard/whoami", GetWhoAmI);
        app.MapGet("/debug/db", TestConnectionString);
        return app;
    }

    private static async Task<string?> TestConnectionString(NpgsqlDataSource dataSource, CancellationToken token)
    {
        await using var conn = await dataSource.OpenConnectionAsync(token);
        return await conn.ExecuteScalarAsync<string>("SELECT version()");
    }


    private static IResult LifeTimes(TransientProbe t1, ScopedProbe s1, SingletonProbe g1, HttpContext ctx)
    {
        var sp = ctx.RequestServices;
        var t2 = sp.GetRequiredService<TransientProbe>();
        var s2 = sp.GetRequiredService<ScopedProbe>();
        var g2 = sp.GetRequiredService<SingletonProbe>();

        return TypedResults.Ok(new
        {
            transient = new[] { t1.Id, t2.Id },
            scoped = new[] { s1.Id, s2.Id },
            singleton = new[] { g1.Id, g2.Id },
        });
    }

    private static string Correlation(RequestContext ctx)
    {
        return ctx.CorrelationId;
    }

    private static IResult GetWhoAmI(LeaderboardCache cache, RequestContext rc)
    {
        return TypedResults.Ok(new
        {
            cacheSees = cache.WhoAmI(rc.CorrelationId),
            requestHas = rc.InstanceId,
            correlationId = rc.CorrelationId
        });
    }
    
    
}