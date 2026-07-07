namespace AIService.AI.Options;

public sealed class OpenAiOptions
{
    public const string SectionName = "OpenAI";

    public string ApiKey { get; init; } = string.Empty;

    public string ChatModel { get; init; } = "gpt-4.1-mini";

    public string EmbeddingModel { get; init; } =
        "text-embedding-3-small";
}
