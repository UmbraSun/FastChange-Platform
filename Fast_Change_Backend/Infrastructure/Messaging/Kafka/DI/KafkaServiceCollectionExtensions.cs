using Application.Common.Interfaces;
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

        services.AddSingleton<KafkaProducerFactory>();

        services.AddSingleton<IKafkaProducer>(sp =>
        {
            var factory = sp.GetRequiredService<KafkaProducerFactory>();
            return new KafkaProducer(factory);
        });

        services.AddSingleton<IProducer<string, string>>(sp =>
        {
            var factory = sp.GetRequiredService<KafkaProducerFactory>();
            return factory.Producer;
        });

        services.AddSingleton<IAdminClient>(sp =>
        {
            var factory = sp.GetRequiredService<KafkaProducerFactory>();
            return factory.AdminClient;
        });

        services.AddSingleton<IConsumer<string, string>>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<KafkaSettings>>().Value;

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
