using Application.Common.Interfaces;
using Confluent.Kafka;
using Infrastructure.Messaging.Kafka.Consumers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Messaging.Kafka.DI;

public static class KafkaServiceCollectionExtensions
{
    public static IServiceCollection AddKafka(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<KafkaSettings>(
            configuration.GetSection(KafkaSettings.SectionName));

        var settings = configuration
            .GetSection(KafkaSettings.SectionName)
            .Get<KafkaSettings>()!;

        services.AddSingleton<IProducer<string, string>>(sp =>
        {
            var config = new ProducerConfig
            {
                BootstrapServers = settings.BootstrapServers,
                ClientId = settings.ClientId,
                Acks = settings.Acks,
                EnableIdempotence = settings.EnableIdempotence,
                CompressionType = settings.CompressionType,
                MessageTimeoutMs = settings.MessageTimeoutMs
            };

            return new ProducerBuilder<string, string>(config).Build();
        });

        services.AddSingleton<IConsumer<string, string>>(sp =>
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = settings.BootstrapServers,
                GroupId = settings.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            return new ConsumerBuilder<string, string>(config).Build();
        });

        return services;
    }
}
