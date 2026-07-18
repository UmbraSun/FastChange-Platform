using Npgsql;
using Respawn;

namespace IntegrationTests.Infrastructure;

public sealed class DatabaseReset
{
    private readonly string _connectionString;
    private Respawner? _respawner;

    public DatabaseReset(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InitializeAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();

        _respawner = await Respawner.CreateAsync(connection,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = ["public"]
            });
    }

    public async Task ResetAsync()
    {
        ArgumentNullException.ThrowIfNull(_respawner);
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
    }
}
