using BuildingBlocks.Messaging;
using Contracts.Enums;
using Contracts.Events;
using TelegramBot.Services;

namespace TelegramBot.Handlers;

public sealed class TransactionCompletedTelegramHandler
    : IIntegrationEventHandler<TransactionCompletedEvent>
{
    private readonly TelegramNotificationService _notification;

    public TransactionCompletedTelegramHandler(
        TelegramNotificationService notification)
    {
        _notification = notification;
    }

    public Task HandleAsync(
        TransactionCompletedEvent @event,
        CancellationToken ct)
    {
        var message = @event.Type switch
        {
            TransactionType.Exchange => "Exchange completed\n\n" +
                $"Sold: {@event.Amount} {@event.ToCurrency}\n" +
                $"Received: {@event.ReceivedAmount} {@event.ToCurrency}\n" +
                $"Rate: {@event.ExchangeRate}",
            TransactionType.Transfer => "Transfer completed\n\n" +
                $"Amount: {@event.Amount} {@event.FromCurrency}",
            _ => "Transaction completed"
        };

        return _notification.SendMessageAsync(message, ct);
    }
}
