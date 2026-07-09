using Microsoft.Extensions.Diagnostics.HealthChecks;
using Qdrant.Client;

namespace AIService.Infrastructure.HealthChecks;

public sealed class QdrantHealthCheck
    : IHealthCheck
{
    private readonly QdrantClient _client;

    public QdrantHealthCheck(
        QdrantClient client)
    {
        _client = client;
    }


    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _client.ListCollectionsAsync(
                cancellationToken);


            return HealthCheckResult.Healthy(
                "Qdrant is available");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                "Qdrant is unavailable",
                ex);
        }
    }
}
