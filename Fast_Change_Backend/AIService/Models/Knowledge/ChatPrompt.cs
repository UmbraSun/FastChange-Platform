namespace AIService.Models.Knowledge;

public sealed record ChatPrompt(
    string SystemPrompt,
    string UserPrompt);
