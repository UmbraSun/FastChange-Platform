using AIService.AI.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace AIService.Infrastructure.HealthChecks;

public sealed class OpenAiHealthCheck
    : IHealthCheck
{
    private readonly OpenAiOptions _options;

    public OpenAiHealthCheck(
        IOptions<OpenAiOptions> options)
    {
        _options = options.Value;
    }


    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(
            _options.ApiKey))
        {
            return Task.FromResult(
                HealthCheckResult.Unhealthy(
                    "OpenAI API key is missing"));
        }


        return Task.FromResult(
            HealthCheckResult.Healthy(
                "OpenAI configured"));
    }
}
