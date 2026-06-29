namespace Application.Common.Interfaces;

/// <summary>
/// Represents a publisher for sending messages to RabbitMQ.
/// </summary>
public interface IRabbitMqPublisher
{
    /// <summary>
    /// Publishes a message to the specified exchange with the given routing key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="exchange"></param>
    /// <param name="routingKey"></param>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task PublishAsync<T>(
        string exchange,
        string routingKey,
        T message,
        CancellationToken cancellationToken = default);
}
