using Microsoft.Extensions.Primitives;

namespace Arcadia.Api.Infrastructure;

public class CorrelationIdMiddleware
{
    private const string Header = "X-Correlation-Id";
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

    // Scoped services go HERE, as InvokeAsync parameters, not in the constructor.
    public async Task InvokeAsync(HttpContext context, RequestContext requestContext)
    {
        var correlationId = context.Request.Headers[Header].ToString();
        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
        }
        
        requestContext.CorrelationId = correlationId;
        
        context.Response.Headers[Header] = correlationId;   
        
        await _next(context);
        
        
    }
}