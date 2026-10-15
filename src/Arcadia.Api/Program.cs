using Arcadia.Api.Middleware;
using Arcadia.Api.Endpoints;
using Arcadia.Api.Options;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"MaxRange from config: {builder.Configuration["Leaderboards:MaxRange"]}");

builder.Services.AddOptions<LeaderboardOptions>()
    .Bind(builder.Configuration.GetSection("Leaderboards"))
    .ValidateDataAnnotations()
    .Validate(o => o.DefaultRange <= o.MaxRange, "DefaultRange must not exceed MaxRange")
    .Validate(o => o.DefaultLimit <= o.MaxLimit, "DefaultLimit must not exceed MaxLimit")
    .ValidateOnStart();

var app = builder.Build();

app.UseMiddleware<RequestTimingMiddleware>();
app.MapPlayerEndpoints();
app.MapShopEndpoints();
app.MapLeaderboardEndpoints();

app.MapGet("/", () => "Hello World!");

app.Run();
