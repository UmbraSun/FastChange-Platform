using AIService.AI.Options;
using Microsoft.Extensions.Options;
using OpenAI;

namespace AIService.Providers.OpenAI;

public sealed class OpenAiEmbeddingProvider
    : IEmbeddingProvider
{
    private readonly OpenAIClient _client;
    private readonly OpenAiOptions _options;

    public OpenAiEmbeddingProvider(
        OpenAIClient client,
        IOptions<OpenAiOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public Task<float[]> CreateEmbeddingAsync(
        string text,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
