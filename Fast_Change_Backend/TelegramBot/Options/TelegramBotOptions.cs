namespace TelegramBot.Options;

public sealed class TelegramBotOptions
{
    public const string SectionName = "TelegramBot";

    public string BotToken { get; init; } = string.Empty;

    public long DefaultChatId { get; init; }
}
