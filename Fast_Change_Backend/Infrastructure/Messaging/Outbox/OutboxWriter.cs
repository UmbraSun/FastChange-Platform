using Application.Common.Interfaces;
using Application.Common.Models;
using Contracts.Events;
using System.Text.Json;

namespace Infrastructure.Messaging.Outbox;

public sealed class OutboxWriter 
    : IOutboxWriter
{
    private readonly IOutboxRepository _repository;

    public OutboxWriter(IOutboxRepository repository)
    {
        _repository = repository;
    }

    public Task AddAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
    {
        var message = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = typeof(TEvent).FullName!,
            Payload = JsonSerializer.Serialize(@event),
            Topic = ResolveTopic<TEvent>(),
            Key = Guid.NewGuid().ToString(),
            OccurredOnUtc = DateTime.UtcNow
        };

        return _repository.AddAsync(message, cancellationToken);
    }

    private static string ResolveTopic<TEvent>()
    {
        if (typeof(TEvent) == typeof(ExchangeCompletedEvent))
            return "exchange-events";

        throw new InvalidOperationException($"Kafka topic is not configured for {typeof(TEvent).Name}");
    }
}
