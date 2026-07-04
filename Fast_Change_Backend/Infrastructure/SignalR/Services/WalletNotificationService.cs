using Application.Common.Interfaces;
using Infrastructure.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.SignalR.Services;

public sealed class WalletNotificationService
    : IWalletNotificationService
{
    private readonly IHubContext<WalletHub> _hub;

    public WalletNotificationService(
        IHubContext<WalletHub> hub)
    {
        _hub = hub;
    }

    public async Task WalletUpdatedAsync(
        Guid walletId,
        CancellationToken cancellationToken)
    {
        await _hub.Clients
            .User(walletId.ToString())
            .SendAsync(
                "WalletUpdated",
                new
                {
                    WalletId = walletId
                },
                cancellationToken);
    }
}
