using Arcadia.Api.Infrastructure;

namespace Arcadia.Api.Leaderboards;

public class LeaderboardCache
{
    private readonly RequestContext _requestContext;

    // BUG (deliberate): singleton taking a scoped dependency
    public LeaderboardCache(RequestContext requestContext)
        => _requestContext = requestContext;

    public string WhoAmI() => _requestContext.CorrelationId;
}