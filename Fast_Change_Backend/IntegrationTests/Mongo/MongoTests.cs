using FluentAssertions;
using Infrastructure.Mongo.Documents;
using IntegrationTests.Infrastructure;
using MongoDB.Driver;

namespace IntegrationTests.Mongo;

[Collection(nameof(MongoCollection))]
public sealed class MongoTests
{
    private readonly IntegrationFixture _fixture;

    public MongoTests(IntegrationFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Mongo_Should_Insert_Wallet_History()
    {
        // Arrange
        var operationId = Guid.NewGuid();
        var document = new WalletHistoryDocument
        {
            OperationId = operationId,
            WalletId = Guid.NewGuid(),
            SignedAmount = -100,
            OperationType = "Exchange",
            ExchangeRate = 10,
            ReceivedAmount = 1000,
            CreatedAtUtc = DateTime.UtcNow
        };

        var collection = _fixture.Mongo.Database.GetCollection<WalletHistoryDocument>("wallet-history");

        // Act
        await collection.InsertOneAsync(document);
        var result = await collection.Find(x => x.OperationId == operationId).SingleAsync();

        // Assert
        result.Should().NotBeNull();
        result.OperationId.Should().Be(operationId);
        result.WalletId.Should().Be(document.WalletId);
        result.SignedAmount.Should().Be(-100);
        result.OperationType.Should().Be("Exchange");
        result.ExchangeRate.Should().Be(10);
        result.ReceivedAmount.Should().Be(1000);
    }
}
