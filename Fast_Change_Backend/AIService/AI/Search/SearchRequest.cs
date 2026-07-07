namespace AIService.AI.Search;

public sealed record SearchRequest(
    string Query,
    int Top = 5);
