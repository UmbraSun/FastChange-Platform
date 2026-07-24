namespace IntegrationTests.Infrastructure;

[CollectionDefinition(nameof(ExchangeRateCollection))]
public sealed class ExchangeRateCollection
    : ICollectionFixture<ExchangeRateFailureFactory>
{
}
