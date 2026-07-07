namespace AIService.AI.Search;

public sealed record SearchResult(
    string Id,
    string Text,
    float Score);
