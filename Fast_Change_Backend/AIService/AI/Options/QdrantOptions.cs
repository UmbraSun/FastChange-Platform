namespace AIService.AI.Options;

public sealed class QdrantOptions
{
    public const string SectionName = "Qdrant";

    public string Host { get; init; } = "localhost";

    public int Port { get; init; } = 6334;

    public string CollectionName { get; init; }
        = "knowledge";

    public int VectorSize { get; init; } = 1536;
}
