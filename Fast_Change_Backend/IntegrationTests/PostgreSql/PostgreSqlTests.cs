using FluentAssertions;
using IntegrationTests.Infrastructure;
using Npgsql;
using System.Data;

namespace IntegrationTests.PostgreSql;

[Collection(nameof(DatabaseCollection))]
public sealed class PostgreSqlTests
{
    private readonly IntegrationFixture _fixture;

    public PostgreSqlTests(IntegrationFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task PostgreSql_Should_Start()
    {
        await using var connection = new NpgsqlConnection(_fixture.PostgreSql.ConnectionString);
        await connection.OpenAsync();
        connection.State.Should().Be(ConnectionState.Open);
    }

    [Fact]
    public async Task Database_Should_Respond()
    {
        await using var connection = new NpgsqlConnection(_fixture.PostgreSql.ConnectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("SELECT version()", connection);

        var result = (string?)await command.ExecuteScalarAsync();
        result.Should().Contain("PostgreSQL");
    }
}
