namespace Infrastructure.ExchangeRates.Exceptions;

public sealed class HistoricalRateNotAvailableException : Exception
{
    public HistoricalRateNotAvailableException(string fromCurrency, string toCurrency, DateOnly date)
        : base($"Historical exchange rate {fromCurrency} -> {toCurrency} " + $"is not available for {date:yyyy-MM-dd}.")
    {
    }
}
