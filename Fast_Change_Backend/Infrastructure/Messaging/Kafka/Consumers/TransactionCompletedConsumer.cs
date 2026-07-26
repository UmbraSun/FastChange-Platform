using Confluent.Kafka;
using Contracts.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging.Kafka.Consumers;

public sealed class TransactionCompletedConsumer
    : BaseKafkaConsumer<TransactionCompletedEvent>
{
    protected override string Topic =>
        "transaction-events";

    public TransactionCompletedConsumer(
        IConsumer<string, string> consumer,
        IServiceScopeFactory scopeFactory,
        ILogger<TransactionCompletedConsumer> logger)
        : base(consumer, scopeFactory, logger)
    {
    }
}
