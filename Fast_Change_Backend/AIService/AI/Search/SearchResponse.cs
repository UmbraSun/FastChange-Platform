namespace AIService.AI.Search;

public sealed record SearchResponse(
    IReadOnlyCollection<SearchResult> Results);
