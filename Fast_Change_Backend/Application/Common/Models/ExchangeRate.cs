namespace Application.Common.Models;

public sealed record ExchangeRate(
    string FromCurrency,
    string ToCurrency,
    decimal Rate,
    DateTime RetrievedAtUtc);
