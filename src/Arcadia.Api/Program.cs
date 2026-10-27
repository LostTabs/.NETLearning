using Arcadia.Api.Diagnostics;
using Arcadia.Api.Middleware;
using Arcadia.Api.Endpoints;
using Arcadia.Api.Infrastructure;
using Arcadia.Api.Leaderboards;
using Arcadia.Api.Options;
using Arcadia.Api.Players;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

builder.Services.AddOptions<LeaderboardOptions>()
    .Bind(builder.Configuration.GetSection("Leaderboards"))
    .ValidateDataAnnotations()
    .Validate(o => o.DefaultRange <= o.MaxRange, "DefaultRange must not exceed MaxRange")
    .Validate(o => o.DefaultLimit <= o.MaxLimit, "DefaultLimit must not exceed MaxLimit")
    .ValidateOnStart();

var connectionString = builder.Configuration.GetConnectionString("Arcadia");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Could not resolve Arcadia connection string");
}
builder.Services.AddSingleton(sp => NpgsqlDataSource.Create(connectionString));
builder.Services.AddTransient<TransientProbe>();
builder.Services.AddScoped<ScopedProbe>();
builder.Services.AddScoped<RequestContext>();
builder.Services.AddSingleton<SingletonProbe>();
/* Because we only have one instance of the app running, and no database - everything is managed on one machine memory,
 and we want this repository to last for the lifetime of the app, it makes sense to register it as a singleton - for now. 
 */
builder.Services.AddSingleton<IPlayerRepository, InMemoryPlayerRepository>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<LeaderboardCache>();

var app = builder.Build();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<RequestTimingMiddleware>();
app.MapPlayerEndpoints();
app.MapShopEndpoints();
app.MapLeaderboardEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapDiagnosticsEndpoints();
}

app.MapGet("/", () => "Hello World!");

app.Run();

public record CreatePlayerRequest(string Username);