namespace AIService.Models.Knowledge;

public sealed record KnowledgeSearchResult(
    Guid Id,
    string DocumentName,
    string Heading,
    int ChunkIndex,
    string Content,
    float Score);
