using Contracts.Events;

namespace Application.Common.Interfaces;

/// <summary>
/// Wallet history writer interface for adding transaction events to the wallet history.
/// </summary>
public interface IWalletHistoryWriter
{
    /// <summary>
    /// Adds a transaction event to the wallet history asynchronously.
    /// </summary>
    /// <param name="event"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task AddTransactionAsync(
        TransactionCompletedEvent @event,
        CancellationToken ct);
}
