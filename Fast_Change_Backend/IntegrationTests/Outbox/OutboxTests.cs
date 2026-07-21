using Application.Common.Models;
using Contracts.Events;
using FluentAssertions;
using IntegrationTests.Api;
using IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Outbox;

public sealed class OutboxTests
    : IntegrationTestBase
{
    public OutboxTests(IntegrationFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task Outbox_Should_Save_And_Read_Unprocessed_Message()
    {
        // Arrange
        var messageId = Guid.NewGuid();

        await ExecuteScopeAsync(async db =>
        {
            var message = new OutboxMessage
            {
                Id = messageId,
                Type = nameof(ExchangeCompletedEvent),
                Payload = "{\"operationId\":\"test\"}",
                Topic = "exchange-events",
                Key = messageId.ToString(),
                OccurredOnUtc = DateTime.UtcNow
            };

            db.OutboxMessages.Add(message);
            await db.SaveChangesAsync();
        });

        // Act
        var messages = await ExecuteScopeAsync(async db => 
            await db.OutboxMessages.Where(x => x.ProcessedOnUtc == null).ToListAsync());

        // Assert
        messages.Should().ContainSingle();

        var outbox = messages.Single();
        outbox.Id.Should().Be(messageId);
        outbox.Type.Should().Be(nameof(ExchangeCompletedEvent));
        outbox.Topic.Should().Be("exchange-events");
        outbox.Payload.Should().NotBeNullOrWhiteSpace();
        outbox.ProcessedOnUtc.Should().BeNull();
    }

    [Fact]
    public async Task Outbox_Should_Mark_Message_As_Processed()
    {
        // Arrange
        var messageId = Guid.NewGuid();

        await ExecuteScopeAsync(async db =>
        {
            db.OutboxMessages.Add(
                new OutboxMessage
                {
                    Id = messageId,
                    Type = nameof(ExchangeCompletedEvent),
                    Payload = "{}",
                    Topic = "exchange-events",
                    Key = messageId.ToString(),
                    OccurredOnUtc = DateTime.UtcNow
                });

            await db.SaveChangesAsync();
        });

        // Act
        await ExecuteScopeAsync(async db =>
        {
            var message = await db.OutboxMessages.SingleAsync(x => x.Id == messageId);
            message.ProcessedOnUtc = DateTime.UtcNow;
            await db.SaveChangesAsync();
        });

        // Assert
        await ExecuteScopeAsync(async db =>
        {
            var message = await db.OutboxMessages.SingleAsync(x => x.Id == messageId);
            message.ProcessedOnUtc.Should().NotBeNull();
        });
    }
}
