using Application.Common.Interfaces;
using Contracts.Enums;
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
        _collection = db.GetCollection<WalletHistoryDocument>(options.Value.WalletHistoryCollection);
    }

    public async Task AddTransactionAsync(
        TransactionCompletedEvent @event,
        CancellationToken ct)
    {
        var documents = new List<WalletHistoryDocument>();

        switch (@event.Type)
        {
            case TransactionType.Deposit:
                documents.Add(
                    CreateDocument(
                        @event,
                        @event.ToWalletId,
                        @event.ReceivedAmount ?? @event.Amount,
                        @event.ToBalanceAfter));
                break;

            case TransactionType.Withdraw:
                documents.Add(
                    CreateDocument(
                        @event,
                        @event.FromWalletId,
                        -@event.Amount,
                        @event.FromBalanceAfter));
                break;

            case TransactionType.Transfer:
                documents.Add(
                    CreateDocument(
                        @event,
                        @event.FromWalletId,
                        -@event.Amount,
                        @event.FromBalanceAfter));

                documents.Add(
                    CreateDocument(
                        @event,
                        @event.ToWalletId,
                        @event.Amount,
                        @event.ToBalanceAfter));
                break;

            case TransactionType.Exchange:
                documents.Add(
                    CreateDocument(
                        @event,
                        @event.FromWalletId,
                        -@event.Amount,
                        @event.FromBalanceAfter));

                documents.Add(
                    CreateDocument(
                        @event,
                        @event.ToWalletId,
                        @event.ReceivedAmount ?? 0m,
                        @event.ToBalanceAfter));
                break;

            default: 
                throw new InvalidOperationException($"Unsupported transaction type: {@event.Type}");
        }

        await _collection.InsertManyAsync(
            documents,
            new InsertManyOptions { IsOrdered = false },
            ct);
    }

    private static WalletHistoryDocument CreateDocument(
        TransactionCompletedEvent @event,
        Guid walletId,
        decimal signedAmount,
        decimal balanceAfter)
    {
        return new WalletHistoryDocument
        {
            OperationId = @event.OperationId,
            WalletId = walletId,
            SignedAmount = signedAmount,
            BalanceAfter = balanceAfter,
            OperationType = @event.Type.ToString(),
            ExchangeRate = @event.ExchangeRate,
            ReceivedAmount = @event.ReceivedAmount,
            CreatedAtUtc = @event.CreatedAtUtc
        };
    }
}
