using Application.Common.Interfaces;
using Contracts.Events;
using Contracts.Notifications;

namespace Infrastructure.Notifications;

public sealed class NotificationDispatcher : INotificationDispatcher
{
    private readonly IEnumerable<IExchangeNotificationChannel> _channels;

    public NotificationDispatcher(
        IEnumerable<IExchangeNotificationChannel> channels)
    {
        _channels = channels;
    }

    public async Task DispatchExchangeCompletedAsync(
        ExchangeCompletedEvent @event,
        CancellationToken cancellationToken)
    {
        foreach (var channel in _channels)
        {
            await channel.NotifyAsync(@event, cancellationToken);
        }
    }

}
