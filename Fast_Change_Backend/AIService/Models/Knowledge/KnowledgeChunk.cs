namespace AIService.Models.Knowledge;

public sealed record KnowledgeChunk(
    Guid Id,
    string DocumentName,
    string Heading,
    string Content);
