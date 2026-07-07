namespace AIService.AI.Chat;

public sealed record ChatResponse(
    string Answer,
    IReadOnlyCollection<string> Sources);
