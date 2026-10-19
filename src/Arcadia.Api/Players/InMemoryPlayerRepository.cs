using System.Collections.Concurrent;

namespace Arcadia.Api.Players;

public class InMemoryPlayerRepository : IPlayerRepository
{
    private readonly ConcurrentDictionary<Guid, Player> _players = new();
    private readonly ConcurrentDictionary<string, Guid> _userNames = new(StringComparer.OrdinalIgnoreCase);
    
    public Task<Player?> GetAsync(Guid id, CancellationToken ct)
    {
        _players.TryGetValue(id, out var player);
        return Task.FromResult(player);
    }

    public Task<bool> TryAddAsync(Player player, CancellationToken ct)
    {
        /*
         * In an actual use case, we want to make sure that players have unique usernames.
         * By checking if the username is already taken (which is an atomic function), if 2 players
         * are trying to register with the same username, it would be stopped there.
         */
        if (!_userNames.TryAdd(player.Username, player.Id))
        {
            return Task.FromResult(false);
        }

        if (_players.TryAdd(player.Id, player))
        {
            return Task.FromResult(true);
        }
        
        _userNames.TryRemove(player.Username, out _);
        return Task.FromResult(false);

    }
}