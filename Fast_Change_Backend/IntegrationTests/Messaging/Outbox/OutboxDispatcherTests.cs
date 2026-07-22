using Application.Common.Models;
using Contracts.Events;
using FluentAssertions;
using IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using System.Text.Json;

namespace IntegrationTests.Messaging.Outbox;

public sealed class OutboxDispatcherTests
    : MessagingTestBase
{
    public OutboxDispatcherTests(IntegrationFixture fixture)
        : base(fixture)
    {
    }


    [Fact]
    public async Task OutboxDispatcher_Should_Publish_Message_To_Kafka()
    {
        // Arrange
        var eventId = Guid.NewGuid();

        await using (var scope = Services.CreateAsyncScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.OutboxMessages.Add(new OutboxMessage
            {
                Id = eventId,
                Type = nameof(ExchangeCompletedEvent),
                Payload = JsonSerializer.Serialize(
                    new ExchangeCompletedEvent(
                        Guid.NewGuid(),
                        Guid.NewGuid(),
                        Guid.NewGuid(),
                        100m,
                        10m,
                        1000m)),
                Topic = "exchange-events",
                Key = eventId.ToString(),
                OccurredOnUtc = DateTime.UtcNow
            });

            await db.SaveChangesAsync();
        }

        using var consumer = new KafkaTestConsumer(Fixture.Kafka.BootstrapServers);
        consumer.Subscribe("exchange-events");

        // Act
        var message = consumer.Consume(TimeSpan.FromSeconds(10));

        // Assert
        message.Should().NotBeNull();
        message!.Message.Key.Should().Be(eventId.ToString());
        message.Message.Value.Should().Contain(nameof(ExchangeCompletedEvent));

        await ExecuteScopeAsync(async db =>
        {
            var outbox = await db.OutboxMessages.SingleAsync(x => x.Id == eventId);
            outbox.ProcessedOnUtc.Should().NotBeNull();
        });
    }
}
