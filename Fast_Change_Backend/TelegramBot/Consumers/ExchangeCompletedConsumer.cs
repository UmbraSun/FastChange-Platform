using Confluent.Kafka;
using Contracts.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using TelegramBot.Handlers;
using TelegramBot.Services;

namespace TelegramBot.Consumers;

public sealed class ExchangeCompletedConsumer : BackgroundService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IServiceScopeFactory _scopeFactory;

    public ExchangeCompletedConsumer(
        IConsumer<string, string> consumer,
        IServiceScopeFactory scopeFactory)
    {
        _consumer = consumer;
        _scopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("exchange-events");

        return Task.Run(() =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(stoppingToken);

                var @event = JsonSerializer.Deserialize<ExchangeCompletedEvent>(
                    result.Message.Value)!;

                using var scope = _scopeFactory.CreateScope();

                var handler = scope.ServiceProvider
                    .GetRequiredService<ExchangeCompletedTelegramHandler>();

                handler.HandleAsync(@event, stoppingToken)
                    .GetAwaiter()
                    .GetResult();

                _consumer.Commit(result);
            }
        }, stoppingToken);
    }
}
