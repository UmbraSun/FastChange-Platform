using Application.Common.Models;
using Contracts.Enums;
using Contracts.Events;
using FluentAssertions;
using Infrastructure.BackgroundServices.Outbox;
using IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                        TransactionType.Exchange,
                        1000m)),
                Topic = "transaction-events",
                Key = eventId.ToString(),
                OccurredOnUtc = DateTime.UtcNow
            });

            await db.SaveChangesAsync();
        });

        using var consumer = new KafkaTestConsumer(Fixture.Kafka.BootstrapServers);
        consumer.Subscribe("transaction-events");

        var message = consumer.Consume(TimeSpan.FromSeconds(10));
        message.Should().NotBeNull();
        message!.Message.Key.Should().Be(eventId.ToString());
        message.Message.Value.Should().Contain(nameof(TransactionCompletedEvent));

        await ExecuteScopeAsync(async db =>
        {
            var outbox = await db.OutboxMessages.SingleAsync(x => x.Id == eventId);
            outbox.ProcessedOnUtc.Should().NotBeNull();
        });
    }

    [Fact]
    public void OutboxDispatcher_Should_Be_Registered()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();

        // Act
        var hostedServices = scope.ServiceProvider.GetServices<IHostedService>();

        // Assert
        hostedServices.Should().ContainSingle(x => x is OutboxDispatcher);
    }
}
