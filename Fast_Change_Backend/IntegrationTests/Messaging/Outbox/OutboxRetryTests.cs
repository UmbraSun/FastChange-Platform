using Application.Common.Models;
using Contracts.Enums;
using Contracts.Events;
using FluentAssertions;
using IntegrationTests.Infrastructure;
using IntegrationTests.Infrastructure.Fakes;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace IntegrationTests.Messaging.Outbox;

[Collection(nameof(MessagingCollection))]
public sealed class OutboxRetryTests : OutboxTestBase
{
    public OutboxRetryTests(
        IntegrationFixture fixture)
        : base(fixture)
    {
    }


    [Fact]
    public async Task OutboxDispatcher_Should_Keep_Message_When_Publish_Fails_And_Process_After_Recovery()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        FailingKafkaProducer.ShouldFail = true;

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
                        null,
                        "USD",
                        "EUR",
                        TransactionType.Transfer)),
                Topic = "transaction-events",
                Key = eventId.ToString(),
                OccurredOnUtc = DateTime.UtcNow
            });

            await db.SaveChangesAsync();
        });


        // Act
        await Task.Delay(1500);

        // Assert failed publish
        await ExecuteScopeAsync(async db =>
        {
            var message = await db.OutboxMessages.SingleAsync(x => x.Id == eventId);
            message.ProcessedOnUtc.Should().BeNull();
        });

        // Recover Kafka
        FailingKafkaProducer.ShouldFail = false;

        // Wait next dispatcher cycle
        await Task.Delay(1500);

        // Assert retry succeeded
        await ExecuteScopeAsync(async db =>
        {
            var message = await db.OutboxMessages.SingleAsync(x => x.Id == eventId);
            message.ProcessedOnUtc.Should().NotBeNull();
        });


        FailingKafkaProducer.ShouldFail = false;
    }
}
