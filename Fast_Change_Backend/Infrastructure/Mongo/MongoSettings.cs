namespace Infrastructure.Mongo;

public sealed class MongoSettings
{
    public const string SectionName = "Mongo";

    public string ConnectionString { get; init; } = string.Empty;

    public string DatabaseName { get; init; } = string.Empty;

    public string WalletHistoryCollection { get; init; }
        = "wallet-history";
}
