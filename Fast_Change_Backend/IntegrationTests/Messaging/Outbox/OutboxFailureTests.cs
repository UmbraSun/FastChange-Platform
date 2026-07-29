using Application.Common.Models;
using Contracts.Enums;
using Contracts.Events;
using FluentAssertions;
using IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace IntegrationTests.Messaging.Outbox;

public sealed class OutboxFailureTests
    : OutboxFailureTestBase
{
    public OutboxFailureTests(IntegrationFixture fixture)
        : base(fixture)
    {
    }


    [Fact]
    public async Task OutboxDispatcher_Should_Keep_Message_When_Kafka_Fails()
    {
        // Arrange
        var eventId = Guid.NewGuid();

        await ExecuteScopeAsync(async db =>
        {
            db.OutboxMessages.Add(new OutboxMessage
            {
                Id = eventId,
                Type = nameof(TransactionCompletedEvent),
                Payload = JsonSerializer.Serialize(
                    new TransactionCompletedEvent(
                        Guid.NewGuid(),
                        Guid.NewGuid(),
                        Guid.NewGuid(),
                        100m,
                        10m,
                        "USD",
                        "EUR",
                        TransactionType.Exchange,
                        1000m)),
                Topic = "transaction-events",
                Key = eventId.ToString(),
                OccurredOnUtc = DateTime.UtcNow
            });

            await db.SaveChangesAsync();
        });

        // Act
        await Task.Delay(TimeSpan.FromSeconds(3));

        // Assert
        await ExecuteScopeAsync(async db =>
        {
            var message = await db.OutboxMessages.SingleAsync(x => x.Id == eventId);
            message.ProcessedOnUtc.Should().BeNull();
        });
    }
}