namespace Arcadia.Api.Infrastructure;

public class RequestContext
{
    public Guid InstanceId { get; } = Guid.NewGuid();
    public string CorrelationId { get; set; } = "";
}