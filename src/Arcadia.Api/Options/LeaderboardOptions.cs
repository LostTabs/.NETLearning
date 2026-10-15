using System.ComponentModel.DataAnnotations;

namespace Arcadia.Api.Options;

public class LeaderboardOptions
{
    [Range(1,1000)]
    public int DefaultLimit;
    [Range(1,1000)]
    public int MaxLimit;
    [Range(1,1000)]
    public int DefaultRange;
    [Range(1,1000)]
    public int MaxRange;
}