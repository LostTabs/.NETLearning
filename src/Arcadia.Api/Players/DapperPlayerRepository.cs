using System.Diagnostics;
using Dapper;
using Npgsql;

namespace Arcadia.Api.Players;

public class DapperPlayerRepository : IPlayerRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public DapperPlayerRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
    
    public async Task<Player?> GetAsync(long id, CancellationToken ct)
    {
        const string sql = """
                           SELECT id, username, email, country, created_at AS CreatedAt
                           FROM players
                           WHERE id = @id
                           """;

        await using var connection = await _dataSource.OpenConnectionAsync(ct);
        var command = new CommandDefinition(sql, new { id }, cancellationToken: ct);

        return await connection.QuerySingleOrDefaultAsync<Player>(command);
    }

    public async Task<CreatePlayerResult> CreateAsync(string username, string email, string country, CancellationToken ct)
    {
        const string sql = """
                           INSERT INTO players (username, email, country) 
                           VALUES (@username, @email, @country)
                           RETURNING id, username, email, country, created_at AS CreatedAt
                           """;

        await using var connection = await _dataSource.OpenConnectionAsync(ct);
        var command = new CommandDefinition(sql, new {username, email, country}, cancellationToken: ct);

        try
        {
            var player = await connection.QuerySingleAsync<Player>(command);
            return new CreatePlayerResult(CreatePlayerStatus.Created, player);
        }
        catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.UniqueViolation &&
                                           ex.ConstraintName is "players_username_lower_uq" or "players_email_lower_uq")
        {
            return ex.ConstraintName switch
            {
                "players_username_lower_uq" => new CreatePlayerResult(CreatePlayerStatus.UsernameTaken, null),
                "players_email_lower_uq" => new CreatePlayerResult(CreatePlayerStatus.EmailTaken, null),
                _ => throw new UnreachableException()
            };
        }
    }
}