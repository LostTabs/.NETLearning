using Arcadia.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);



var app = builder.Build();


app.UseMiddleware<RequestTimingMiddleware>();

app.MapGet("/", () => "Hello World!");

app.Run();
