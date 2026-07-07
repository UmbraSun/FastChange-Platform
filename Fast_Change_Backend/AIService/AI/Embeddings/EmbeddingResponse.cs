namespace AIService.AI.Embeddings;

public sealed record EmbeddingResponse(
    IReadOnlyList<float> Vector);
