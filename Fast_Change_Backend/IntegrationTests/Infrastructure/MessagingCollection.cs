namespace IntegrationTests.Infrastructure;

[CollectionDefinition(nameof(MessagingCollection))]
public sealed class MessagingCollection
    : ICollectionFixture<IntegrationFixture>
{
}
