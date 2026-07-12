using BuildingBlocks.Messaging;
using Confluent.Kafka;
using Contracts.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infrastructure.Messaging.Kafka.Consumers;

public sealed class ExchangeCompletedConsumer
    : BaseKafkaConsumer<ExchangeCompletedEvent>
{
    protected override string Topic => "exchange-events";

    public ExchangeCompletedConsumer(
        IConsumer<string, string> consumer,
        IServiceScopeFactory scopeFactory,
        ILogger<ExchangeCompletedConsumer> logger)
        : base(consumer, scopeFactory, logger)
    {
    }
}
