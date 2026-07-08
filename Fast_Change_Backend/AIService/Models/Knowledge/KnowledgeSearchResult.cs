namespace AIService.Models.Knowledge;

public sealed record KnowledgeSearchResult(
    Guid Id,
    string DocumentName,
    string Heading,
    string Content,
    float Score);
