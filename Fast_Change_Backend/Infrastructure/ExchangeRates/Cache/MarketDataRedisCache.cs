using Application.Common.Interfaces;
using Application.Common.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.ExchangeRates.Cache;

public sealed class MarketDataRedisCache
    : IMarketDataCache
{
    private const string KeyPrefix = "market-data";
    private readonly IConnectionMultiplexer _redis;

    public MarketDataRedisCache(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task<IReadOnlyList<MarketDataItem>?> GetAsync(
        string key,
        CancellationToken cancellationToken)
    {
        var database = _redis.GetDatabase();
        var value = await database.StringGetAsync($"{KeyPrefix}:{key}");

        if (value.IsNullOrEmpty)
            return null;

        return JsonSerializer.Deserialize<List<MarketDataItem>>(value.ToString());
    }

    public async Task SetAsync(
        string key,
        IReadOnlyList<MarketDataItem> data,
        TimeSpan expiration,
        CancellationToken cancellationToken)
    {
        var database = _redis.GetDatabase();
        var serialized = JsonSerializer.Serialize(data);

        await database.StringSetAsync($"{KeyPrefix}:{key}", serialized, expiration);
    }
}
