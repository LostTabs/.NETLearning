using Arcadia.Api.Infrastructure;

namespace Arcadia.Api.Leaderboards;

public class LeaderboardCache
{
    //This class is here for documentation of a previous lesson where this function was used to show a singleton capturing a scoped service
    public string WhoAmI(string correlationId)
    {
        return correlationId;   
    }
}