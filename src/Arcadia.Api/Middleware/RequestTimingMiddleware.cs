using System.Diagnostics;
using System.Globalization;

namespace Arcadia.Api.Middleware;

public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;

    public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var start = Stopwatch.GetTimestamp();
        
        context.Response.OnStarting(() =>
        {
            context.Response.Headers["X-Response-Time-Ms"] = Stopwatch.GetElapsedTime(start).TotalMilliseconds
                .ToString("F2", CultureInfo.InvariantCulture);
            return Task.CompletedTask;
        });
        try
        {
            await _next(context);
        }
        finally
        {
            var elapsedTime = Stopwatch.GetElapsedTime(start);
         
            _logger.LogInformation("{Method} {Path} responded {StatusCode} in {ElapsedMs} ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                elapsedTime.TotalMilliseconds);    
        }
    }
}