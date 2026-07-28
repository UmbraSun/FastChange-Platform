using Confluent.Kafka;
using Contracts.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging.Kafka.Consumers;

public sealed class ExchangeCompletedConsumer : BaseKafkaConsumer<TransactionCompletedEvent>
{
    public ExchangeCompletedConsumer(IConsumer<string, string> consumer, IServiceScopeFactory scopeFactory, ILogger logger)
        : base(consumer, scopeFactory, logger)
    {
    }

    protected override string Topic => "transaction-events";
}
