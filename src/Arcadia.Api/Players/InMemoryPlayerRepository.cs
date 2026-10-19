using System.Collections.Concurrent;

namespace Arcadia.Api.Players;

public class InMemoryPlayerRepository : IPlayerRepository
{
    private ConcurrentDictionary<Guid, Player> players = new();
    private ConcurrentDictionary<string, Guid> userNames = new(StringComparer.OrdinalIgnoreCase);
    
    public Task<Player?> GetAsync(Guid id, CancellationToken ct)
    {
        players.TryGetValue(id, out var player);
        return Task.FromResult(player);
    }

    public Task<bool> TryAddAsync(Player player, CancellationToken ct)
    {
        /*
         * In an actual use case, we want to make sure that players have unique usernames.
         * By checking if the username is already taken (which is an atomic function), if 2 players
         * are trying to register with the same username, it would be stopped there.
         */
        if (!userNames.TryAdd(player.Username, player.Id))
        {
            return Task.FromResult(false);
        }

        if (players.TryAdd(player.Id, player))
        {
            return Task.FromResult(true);
        }
        
        userNames.TryRemove(player.Username, out _);
        return Task.FromResult(false);

    }
}