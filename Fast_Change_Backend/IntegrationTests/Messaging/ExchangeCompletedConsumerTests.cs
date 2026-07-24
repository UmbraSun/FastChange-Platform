using Application.Common.Interfaces;
using Contracts.Events;
using FluentAssertions;
using Infrastructure.Mongo.Documents;
using IntegrationTests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Text.Json;

namespace IntegrationTests.Messaging;

public sealed class ExchangeCompletedConsumerTests
    : MessagingTestBase
{
    public ExchangeCompletedConsumerTests(IntegrationFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task ExchangeCompletedConsumer_Should_Write_Wallet_History()
    {
        // Arrange
        var operationId = Guid.NewGuid();
        var fromWalletId = Guid.NewGuid();
        var toWalletId = Guid.NewGuid();

        var @event = new ExchangeCompletedEvent(operationId, fromWalletId, toWalletId, 100m, 10m, 1000m);
        var producer = Services.GetRequiredService<IKafkaProducer>();

        // Act
        await producer.PublishAsync(
            topic: "exchange-events",
            key: operationId.ToString(),
            value: JsonSerializer.Serialize(@event),
            headers: null,
            cancellationToken: CancellationToken.None);

        // Assert
        var database = Fixture.Mongo.Database;
        var collection = database.GetCollection<WalletHistoryDocument>("wallet-history");
        var documents = new List<WalletHistoryDocument>();
        var timeout = DateTime.UtcNow.AddSeconds(10);

        while (DateTime.UtcNow < timeout)
        {
            documents = await collection
                .Find(x => x.OperationId == operationId)
                .ToListAsync();

            if (documents.Count == 2) break;

            await Task.Delay(200);
        }

        documents.Should().HaveCount(2);
        documents.Should().Contain(x =>
            x.WalletId == fromWalletId &&
            x.SignedAmount == -100m);
        documents.Should().Contain(x =>
            x.WalletId == toWalletId &&
            x.SignedAmount == 1000m);
    }
}
