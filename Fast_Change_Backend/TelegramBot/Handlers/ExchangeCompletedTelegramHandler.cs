using Contracts.Events;
using TelegramBot.Services;

namespace TelegramBot.Handlers;

public sealed class ExchangeCompletedTelegramHandler
{
    private readonly TelegramNotificationService _notification;

    public ExchangeCompletedTelegramHandler(
        TelegramNotificationService notification)
    {
        _notification = notification;
    }

    public Task HandleAsync(
        ExchangeCompletedEvent @event,
        CancellationToken ct)
    {
        var message =
            "💱 Exchange completed\n\n" +
            $"From: {@event.Amount}\n" +
            $"Received: {@event.ReceivedAmount}\n" +
            $"Rate: {@event.Rate}";

        return _notification.SendMessageAsync(message, ct);
    }
}
