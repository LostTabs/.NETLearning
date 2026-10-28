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

/*
 * Since a future EF Core version of the repository will hold a DbContext, we are adding it as Scoped.
 * Singleton would work since the repository doesnt hold per-request state, but it would force us to change things
 * later.
 */
builder.Services.AddScoped<IPlayerRepository, DapperPlayerRepository>();
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
