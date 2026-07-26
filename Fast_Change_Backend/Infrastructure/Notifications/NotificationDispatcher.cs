using Application.Common.Interfaces;
using Contracts.Events;
using Contracts.Notifications;

namespace Infrastructure.Notifications;

public sealed class NotificationDispatcher : INotificationDispatcher
{
    private readonly IEnumerable<ITransactionNotificationChannel> _channels;

    public NotificationDispatcher(IEnumerable<ITransactionNotificationChannel> channels)
    {
        _channels = channels;
    }

    public async Task DispatchTransactionCompletedAsync(
        TransactionCompletedEvent @event,
        CancellationToken cancellationToken)
    {
        foreach (var channel in _channels)
            await channel.NotifyAsync(@event, cancellationToken);
    }

}
