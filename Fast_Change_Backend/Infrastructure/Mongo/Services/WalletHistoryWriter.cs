using Application.Common.Interfaces;
using Contracts.Events;
using Infrastructure.Mongo.Documents;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Mongo.Services;

public sealed class WalletHistoryWriter : IWalletHistoryWriter
{
    private readonly IMongoCollection<WalletHistoryDocument> _collection;

    public WalletHistoryWriter(
        IMongoDatabase db, 
        IOptions<MongoSettings> options)
    {
        _collection = db.GetCollection<WalletHistoryDocument>(
            options.Value.WalletHistoryCollection);
    }

    public async Task AddExchangeAsync(
        ExchangeCompletedEvent @event,
        CancellationToken ct)
    {
        await _collection.InsertManyAsync(
            [
                new WalletHistoryDocument
                {
                    OperationId = @event.OperationId,
                    WalletId = @event.FromWalletId,
                    SignedAmount = -@event.Amount,
                    OperationType = "Exchange",
                    ExchangeRate = @event.Rate,
                    ReceivedAmount = @event.ReceivedAmount,
                    CreatedAtUtc = DateTime.UtcNow
                },
                new WalletHistoryDocument
                {
                    OperationId = @event.OperationId,
                    WalletId = @event.ToWalletId,
                    SignedAmount = @event.ReceivedAmount,
                    OperationType = "Exchange",
                    ExchangeRate = @event.Rate,
                    ReceivedAmount = @event.ReceivedAmount,
                    CreatedAtUtc = DateTime.UtcNow
                }
            ],
            new InsertManyOptions { IsOrdered = false },
            ct);
    }
}
