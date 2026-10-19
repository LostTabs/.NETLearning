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
        // TODO 1: read the header from context.Request.Headers.
        //         If it's missing or empty, generate a new one (Guid.NewGuid().ToString() is fine).
        var correlationId = context.Request.Headers[Header].ToString();
        if (string.IsNullOrEmpty(correlationId))
        {
            context.Request.Headers[Header] = Guid.NewGuid().ToString();
        }
        
        // TODO 2: store it in requestContext.CorrelationId.
        requestContext.CorrelationId = correlationId;
        
        // TODO 3: add it to context.Response.Headers.
        //         Do this BEFORE calling _next. Once the response body starts,
        //         headers can't be changed.
        context.Response.Headers[Header] = correlationId;   
        
        await _next(context);
        
        
    }
}