using AIService.AI.Options;
using AIService.Models.Knowledge;
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
                        ["hash"] = x.DocumentHash,
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

    public async Task<IReadOnlyCollection<KnowledgeSearchResult>> SearchAsync(
        float[] embedding,
        int limit,
        CancellationToken cancellationToken)
    {
        var result = await _client.SearchAsync(
            collectionName: _options.CollectionName,
            vector: embedding,
            limit: (ulong)limit,
            cancellationToken: cancellationToken);

        return result
            .Select(x => new KnowledgeSearchResult(
                Guid.Parse(x.Id.Uuid),
                x.Payload["document"].StringValue,
                x.Payload["heading"].StringValue,
                x.Payload["content"].StringValue,
                x.Score))
            .ToList();
    }

    public async Task<bool> ExistsAsync(
        string documentName,
        string hash,
        CancellationToken cancellationToken)
    {
        var result =
            await _client.ScrollAsync(
                _options.CollectionName,
                filter: new Filter
                {
                    Must =
                    {
                        new Condition
                        {
                            Field =
                            new FieldCondition
                            {
                                Key = "document",
                                Match =
                                new Match
                                {
                                    Keyword = documentName
                                }
                            }
                        },
                        new Condition
                        {
                            Field =
                            new FieldCondition
                            {
                                Key = "hash",
                                Match =
                                new Match
                                {
                                    Keyword = hash
                                }
                            }
                        }
                    }
                },
                limit: 1,
                cancellationToken:
                    cancellationToken);


        return result.Result.Count > 0;
    }
}
