using Application.Common.Interfaces;
using Contracts.Enums;
using Contracts.Events;
using FluentAssertions;
using Infrastructure.Mongo.Documents;
using IntegrationTests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Text.Json;

namespace IntegrationTests.Messaging;

[Collection(nameof(MessagingCollection))]
public sealed class ExchangeCompletedConsumerIdempotencyTests
    : MessagingTestBase
{
    public ExchangeCompletedConsumerIdempotencyTests(IntegrationFixture fixture)
        : base(fixture)
    {
    }


    [Fact]
    public async Task TransactionCompletedConsumer_Should_Not_Create_Duplicate_History()
    {
        // Arrange
        var operationId = Guid.NewGuid();
        var fromWalletId = Guid.NewGuid();
        var toWalletId = Guid.NewGuid();

        var @event = new TransactionCompletedEvent(
            operationId,
            fromWalletId,
            toWalletId,
            100m,
            1000m,
            "USD",
            "EUR",
            TransactionType.Transfer,
            500m,
            1500m);

        var producer = Factory.Services.GetRequiredService<IKafkaProducer>();
        var payload = JsonSerializer.Serialize(@event);

        // Act
        await producer.PublishAsync(
            "transaction-events",
            operationId.ToString(),
            payload,
            null,
            CancellationToken.None);

        await WaitForHistoryAsync(operationId);

        await producer.PublishAsync(
            "transaction-events",
            operationId.ToString(),
            payload,
            null,
            CancellationToken.None);

        await Task.Delay(2000);

        // Assert
        var database = Factory.Services.GetRequiredService<IMongoDatabase>();
        var collection = database.GetCollection<WalletHistoryDocument>("wallet-history");
        var documents = await collection
            .Find(x => x.OperationId == operationId)
            .ToListAsync();
        documents.Should().HaveCount(2);
    }


    private async Task WaitForHistoryAsync(Guid operationId)
    {
        var database = Factory.Services.GetRequiredService<IMongoDatabase>();
        var collection = database.GetCollection<WalletHistoryDocument>("wallet-history");
        var timeout = DateTime.UtcNow.AddSeconds(10);

        while (DateTime.UtcNow < timeout)
        {
            var count = await collection.CountDocumentsAsync(x => x.OperationId == operationId);

            if (count == 2)
                return;

            await Task.Delay(200);
        }

        throw new TimeoutException("Wallet history was not created");
    }
}

