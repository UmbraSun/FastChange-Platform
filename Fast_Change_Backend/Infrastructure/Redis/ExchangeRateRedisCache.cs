using Application.Common.Interfaces;
using Application.Common.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Redis;

public sealed class ExchangeRateRedisCache : IExchangeRateCache
{
    private readonly IDatabase _db;

    public ExchangeRateRedisCache(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    private static string Key(string from, string to)
        => $"rate:{from.ToUpper()}:{to.ToUpper()}";

    public async Task<ExchangeRate?> GetAsync(string from, string to)
    {
        var value = await _db.StringGetAsync(Key(from, to));

        if (!value.HasValue)
            return null;

        return JsonSerializer.Deserialize<ExchangeRate>(
            value.ToString());
    }

    public async Task SetAsync(ExchangeRate rate, TimeSpan ttl)
    {
        var json = JsonSerializer.Serialize(rate);

        await _db.StringSetAsync(
            Key(rate.FromCurrency, rate.ToCurrency),
            json,
            ttl);
    }
}
