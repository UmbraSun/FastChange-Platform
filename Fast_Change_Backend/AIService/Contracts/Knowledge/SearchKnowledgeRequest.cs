namespace AIService.Contracts.Knowledge;

public sealed record SearchKnowledgeRequest(
    string Query,
    int Top = 5);
