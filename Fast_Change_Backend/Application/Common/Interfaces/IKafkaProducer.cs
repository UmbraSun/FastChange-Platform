namespace Application.Common.Interfaces;

/// <summary>
/// Represents a Kafka producer that can publish messages to Kafka topics.
/// </summary>
public interface IKafkaProducer
{
    /// <summary>
    /// Publishes a message to a specified Kafka topic with the given key, value, and headers.
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="headers"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task PublishAsync(
        string topic,
        string key,
        string value,
        IReadOnlyDictionary<string, string>? headers,
        CancellationToken cancellationToken);
}
