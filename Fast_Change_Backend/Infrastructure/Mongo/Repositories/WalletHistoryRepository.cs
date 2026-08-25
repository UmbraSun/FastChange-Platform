using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.Mongo.Documents;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Mongo.Repositories;

public sealed class WalletHistoryRepository
    : IWalletHistoryReader
{
    private readonly IMongoCollection<WalletHistoryDocument> _collection;

    public WalletHistoryRepository(
        IMongoDatabase database,
        IOptions<MongoSettings> settings)
    {
        _collection = database.GetCollection<WalletHistoryDocument>(settings.Value.WalletHistoryCollection);
    }

    public async Task<IReadOnlyList<WalletHistoryItem>> GetByWalletAsync(
        Guid walletId,
        int take,
        CancellationToken cancellationToken)
    {
        var data = await _collection
            .Find(x => x.WalletId == walletId)
            .SortByDescending(x => x.CreatedAtUtc)
            .Limit(take)
            .ToListAsync(cancellationToken);

        return data.Select(x => new WalletHistoryItem(
            x.OperationId,
            x.WalletId,
            x.SignedAmount,
            x.BalanceAfter,
            x.OperationType,
            x.ExchangeRate,
            x.ReceivedAmount,
            x.CreatedAtUtc))
            .ToList();
    }

    public async Task<IReadOnlyList<WalletHistoryItem>> GetByWalletsAsync(
        IReadOnlyCollection<Guid> walletIds,
        DateTime fromUtc,
        CancellationToken cancellationToken)
    {
        if (walletIds.Count == 0)
            return [];

        var data = await _collection
            .Find(x => walletIds.Contains(x.WalletId) && x.CreatedAtUtc >= fromUtc)
            .SortBy(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return data.Select(x => new WalletHistoryItem(
            x.OperationId,
            x.WalletId,
            x.SignedAmount,
            x.BalanceAfter,
            x.OperationType,
            x.ExchangeRate,
            x.ReceivedAmount,
            x.CreatedAtUtc))
            .ToList();
    }
}
