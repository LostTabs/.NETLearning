using Arcadia.Api.Infrastructure;

namespace Arcadia.Api.Leaderboards;

public class LeaderboardCache
{
    public string WhoAmI(string correlationId)
    {
        return correlationId;   
    }
}