namespace AIService.Contracts.Knowledge;

public sealed record SearchKnowledgeResponse(
    IReadOnlyCollection<SearchKnowledgeItem> Items);

public sealed record SearchKnowledgeItem(
    string Document,
    string Heading,
    int ChunkIndex,
    float Score,
    string Content);
