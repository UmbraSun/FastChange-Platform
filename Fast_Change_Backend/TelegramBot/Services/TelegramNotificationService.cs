using Microsoft.Extensions.Options;
using Telegram.Bot;
using TelegramBot.Options;

namespace TelegramBot.Services;

public sealed class TelegramNotificationService
{
    private readonly TelegramBotClient _client;
    private readonly TelegramBotOptions _options;

    public TelegramNotificationService(
        TelegramBotClient client,
        IOptions<TelegramBotOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public async Task SendMessageAsync(
        string message,
        CancellationToken cancellationToken)
    {
        await _client.SendMessage(
            chatId: _options.DefaultChatId,
            text: message,
            cancellationToken: cancellationToken);
    }
}
