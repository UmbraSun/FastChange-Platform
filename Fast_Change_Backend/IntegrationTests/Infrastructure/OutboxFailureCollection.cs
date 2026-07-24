namespace IntegrationTests.Infrastructure;

[CollectionDefinition(nameof(OutboxFailureCollection))]
public sealed class OutboxFailureCollection
    : ICollectionFixture<OutboxFailureFactory>
{
}
