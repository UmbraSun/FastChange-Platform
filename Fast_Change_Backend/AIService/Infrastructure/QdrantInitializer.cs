using AIService.AI.Options;
using Microsoft.Extensions.Options;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace AIService.Infrastructure;

public sealed class QdrantInitializer
{
    private readonly QdrantClient _client;
    private readonly QdrantOptions _options;

    public QdrantInitializer(
        QdrantClient client,
        IOptions<QdrantOptions> options)
    {
        _client = client;
        _options = options.Value;
    }


    public async Task InitializeAsync(
        CancellationToken cancellationToken)
    {
        var exists =
            await _client.CollectionExistsAsync(
                _options.CollectionName);

        if (exists)
            return;


        await _client.CreateCollectionAsync(
            _options.CollectionName,
            new VectorParams
            {
                Size = (ulong)_options.VectorSize,
                Distance = Distance.Cosine
            },
            cancellationToken: cancellationToken);
    }
}
