using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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

        services.AddSingleton<IAdminClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<KafkaSettings>>().Value;

            var config = new AdminClientConfig
            {
                BootstrapServers = settings.BootstrapServers
            };

            return new AdminClientBuilder(config).Build();
        });

        return services;
    }
}
