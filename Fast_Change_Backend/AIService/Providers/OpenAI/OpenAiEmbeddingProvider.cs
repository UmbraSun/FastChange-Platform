using OpenAI.Embeddings;

namespace AIService.Providers.OpenAI;

public sealed class OpenAiEmbeddingProvider
    : IEmbeddingProvider
{
    private readonly EmbeddingClient _client;

    public OpenAiEmbeddingProvider(
        EmbeddingClient client)
    {
        _client = client;
    }

    public async Task<float[]> CreateEmbeddingAsync(
        string text,
        CancellationToken cancellationToken)
    {
        OpenAIEmbedding embedding =
            await _client.GenerateEmbeddingAsync(
                text,
                cancellationToken: cancellationToken);

        return embedding.ToFloats().ToArray();
    }
}
