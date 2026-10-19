namespace Arcadia.Api.Players;

public interface IPlayerRepository
{
    Task<Player?> GetAsync(Guid id, CancellationToken ct);
    Task<bool> TryAddAsync(Player player, CancellationToken ct); // false = username taken
}