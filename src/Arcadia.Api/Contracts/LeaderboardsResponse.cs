namespace Arcadia.Api.Contracts;

public record PlayerEntry(long PlayerId, int Rank, int Score);

public record LeaderboardsResponse(List<PlayerEntry> Entries);