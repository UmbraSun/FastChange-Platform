namespace AIService.Models.Knowledge;

public sealed record KnowledgeVector(
    Guid Id,
    string DocumentName,
    string DocumentHash,
    string Heading,
    int ChunkIndex,
    string Content,
    DateTime IndexedAtUtc,
    float[] Vector);
