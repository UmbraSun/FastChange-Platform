using Infrastructure.Mongo.Documents;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Mongo;

public sealed class MongoIndexesInitializer
{
    private readonly IMongoDatabase _database;
    private readonly MongoSettings _settings;

    public MongoIndexesInitializer(
        IMongoDatabase database,
        IOptions<MongoSettings> options)
    {
        _database = database;
        _settings = options.Value;
    }

    public async Task InitializeAsync(
        CancellationToken cancellationToken = default)
    {
        var collection = _database.GetCollection<WalletHistoryDocument>(_settings.WalletHistoryCollection);

        var indexKeys = Builders<WalletHistoryDocument>
            .IndexKeys
            .Ascending(x => x.OperationId)
            .Ascending(x => x.WalletId);

        var indexModel = new CreateIndexModel<WalletHistoryDocument>(
            indexKeys,
            new CreateIndexOptions
            {
                Unique = true,
                Name = "ux_wallet_history_operation_wallet"
            });

        await collection.Indexes.CreateOneAsync(
            indexModel,
            cancellationToken: cancellationToken);
    }
}
