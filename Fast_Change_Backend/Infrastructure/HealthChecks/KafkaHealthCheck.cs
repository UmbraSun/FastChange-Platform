using Confluent.Kafka;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Infrastructure.HealthChecks;

public sealed class KafkaHealthCheck
    : IHealthCheck
{
    private readonly IAdminClient _client;

    public KafkaHealthCheck(IAdminClient client)
    {
        _client = client;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var metadata = _client.GetMetadata(TimeSpan.FromSeconds(5));

            return Task.FromResult(
                metadata.Brokers.Count > 0
                    ? HealthCheckResult.Healthy()
                    : HealthCheckResult.Unhealthy());
        }
        catch (Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy(exception: ex));
        }
    }
}
