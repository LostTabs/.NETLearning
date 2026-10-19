using Arcadia.Api.Diagnostics;
using Arcadia.Api.Middleware;
using Arcadia.Api.Endpoints;
using Arcadia.Api.Infrastructure;
using Arcadia.Api.Options;
using Arcadia.Api.Players;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"MaxRange from config: {builder.Configuration["Leaderboards:MaxRange"]}");

builder.Services.AddOptions<LeaderboardOptions>()
    .Bind(builder.Configuration.GetSection("Leaderboards"))
    .ValidateDataAnnotations()
    .Validate(o => o.DefaultRange <= o.MaxRange, "DefaultRange must not exceed MaxRange")
    .Validate(o => o.DefaultLimit <= o.MaxLimit, "DefaultLimit must not exceed MaxLimit")
    .ValidateOnStart();

builder.Services.AddTransient<TransientProbe>();
builder.Services.AddScoped<ScopedProbe>();
builder.Services.AddScoped<RequestContext>();
builder.Services.AddSingleton<SingletonProbe>();
/* Because we only have one instance of the app running, and no database - everything is managed on one machine memory,
 and we want this repository to last for the lifetime of the app, it makes sense to register it as a singleton - for now. 
 */
builder.Services.AddSingleton<IPlayerRepository, InMemoryPlayerRepository>();
builder.Services.AddSingleton(TimeProvider.System);

var app = builder.Build();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<RequestTimingMiddleware>();
app.MapPlayerEndpoints();
app.MapShopEndpoints();
app.MapLeaderboardEndpoints();

app.MapGet("/", () => "Hello World!");

app.MapGet("/di/lifetimes", (
    TransientProbe t1, ScopedProbe s1, SingletonProbe g1, HttpContext ctx) =>
{
    var sp = ctx.RequestServices;
    var t2 = sp.GetRequiredService<TransientProbe>();
    var s2 = sp.GetRequiredService<ScopedProbe>();
    var g2 = sp.GetRequiredService<SingletonProbe>();

    return new
    {
        transient = new[] { t1.Id, t2.Id },
        scoped = new[] { s1.Id, s2.Id },
        singleton = new[] { g1.Id, g2.Id },
    };
});

app.MapGet("/debug/correlation", (RequestContext rc) => rc.CorrelationId);

app.MapGet("/players/{id:guid}", async (Guid id, IPlayerRepository repo, CancellationToken ct) =>
{
    var player = await repo.GetAsync(id, ct);
    return player is null ? Results.NotFound() : Results.Ok(player);
});

app.MapPost("/players", async (
    CreatePlayerRequest request, IPlayerRepository repo,
    TimeProvider time, CancellationToken ct) =>
{
    // TODO: build a new Player (Guid.NewGuid(), the username, time.GetUtcNow())
    var player = new Player(Guid.NewGuid(), request.Username, time.GetUtcNow());
    // TODO: call TryAddAsync
    var result = await repo.TryAddAsync(player, ct);
    // TODO: if it returns false → 409 Conflict (you justified this in 1.1)
    return !result ? Results.Conflict() :
    // TODO: otherwise → 201 Created with a Location header pointing at /players/{id}
    //       Hint: Results.Created(uri, value)
        Results.Created($"/players/{player.Id}", player);
});

app.Run();

public record CreatePlayerRequest(string Username);