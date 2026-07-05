using Contracts.Events;

namespace Application.Common.Interfaces;

/// <summary>
/// Wallet history writer interface for adding exchange events to the wallet history.
/// </summary>
public interface IWalletHistoryWriter
{
    /// <summary>
    /// Adds an exchange event to the wallet history asynchronously.
    /// </summary>
    /// <param name="event"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task AddExchangeAsync(
        ExchangeCompletedEvent @event,
        CancellationToken ct);
}
