using AIService.AI.Options;
using AIService.Models.Knowledge;
using AIService.Services.Knowledge;
using Microsoft.Extensions.Options;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace AIService.Services.VectorStore;

public sealed class QdrantVectorStore
    : IVectorStore
{
    private readonly QdrantClient _client;
    private readonly QdrantOptions _options;


    public QdrantVectorStore(
        QdrantClient client,
        IOptions<QdrantOptions> options)
    {
        _client = client;
        _options = options.Value;
    }


    public async Task UpsertAsync(
        IReadOnlyCollection<KnowledgeVector> vectors,
        CancellationToken cancellationToken)
    {
        var points = vectors
            .Select(x =>
                new PointStruct
                {
                    Id = new PointId
                    {
                        Uuid = x.Id.ToString()
                    },

                    Vectors = x.Vector,

                    Payload =
                    {
                    ["document"] = x.DocumentName,
                    ["heading"] = x.Heading,
                    ["content"] = x.Content
                    }
                })
            .ToList();


        await _client.UpsertAsync(
            _options.CollectionName,
            points,
            cancellationToken: cancellationToken);
    }
}
