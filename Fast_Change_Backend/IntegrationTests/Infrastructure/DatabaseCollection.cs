namespace IntegrationTests.Infrastructure;

[CollectionDefinition(nameof(DatabaseCollection))]
public sealed class DatabaseCollection
    : ICollectionFixture<PostgreSqlFixture>
{
}
