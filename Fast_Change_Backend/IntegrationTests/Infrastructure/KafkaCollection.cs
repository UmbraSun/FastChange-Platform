namespace IntegrationTests.Infrastructure;

[CollectionDefinition(nameof(KafkaCollection))]
public sealed class KafkaCollection
    : ICollectionFixture<KafkaFixture>
{
}
