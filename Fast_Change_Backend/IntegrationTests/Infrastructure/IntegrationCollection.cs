namespace IntegrationTests.Infrastructure;

[CollectionDefinition(nameof(IntegrationCollection))]
public sealed class IntegrationCollection
    : ICollectionFixture<IntegrationFixture>
{
}
