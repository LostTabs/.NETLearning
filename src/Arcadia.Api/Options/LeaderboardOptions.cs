using System.ComponentModel.DataAnnotations;

namespace Arcadia.Api.Options;

public class LeaderboardOptions
{
    [Range(1,1000)]
    public int DefaultLimit { get; set; }
    [Range(1,1000)]
    public int MaxLimit{ get; set; }
    [Range(1,1000)]
    public int DefaultRange{ get; set; }
    [Range(1,1000)]
    public int MaxRange{ get; set; }
}