namespace AIService.Models.Knowledge;

public sealed record KnowledgeVector(
    Guid Id,
    string DocumentName,
    string Heading,
    string Content,
    float[] Vector);
