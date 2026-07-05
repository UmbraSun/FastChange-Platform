using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Mongo.Documents;


public sealed class WalletHistoryDocument
{
    [BsonId]
    public ObjectId Id { get; set; }

    public Guid OperationId { get; set; }

    public Guid WalletId { get; set; }

    public decimal SignedAmount { get; set; }

    public decimal BalanceAfter { get; set; }

    public string OperationType { get; set; } = string.Empty;

    public decimal? ExchangeRate { get; set; }

    public decimal? ReceivedAmount { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}
