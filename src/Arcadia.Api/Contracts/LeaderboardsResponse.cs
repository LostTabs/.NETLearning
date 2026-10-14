namespace Arcadia.Api.Contracts;

public record PlayerEntry(long PlayerId, int rank, int score);

public record LeaderboardsResponse(List<PlayerEntry> leaderboard);