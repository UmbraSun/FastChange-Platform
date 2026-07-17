namespace IntegrationTests.Infrastructure;

[CollectionDefinition(nameof(MongoCollection))]
public sealed class MongoCollection
    : ICollectionFixture<MongoFixture>
{
}
