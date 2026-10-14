using Arcadia.Api.Middleware;
using Arcadia.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);



var app = builder.Build();


app.UseMiddleware<RequestTimingMiddleware>();
app.MapPlayerEndpoints();

app.MapGet("/", () => "Hello World!");

app.Run();
