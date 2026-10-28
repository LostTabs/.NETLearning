namespace Arcadia.Api.Players;

public interface IPlayerRepository
{
    Task<Player?> GetAsync(long id, CancellationToken ct);
    Task<CreatePlayerResult> CreateAsync (string username, string email, string country, CancellationToken ct);
}