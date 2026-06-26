namespace Application.Features.Exchange.PreviewExchange;

public sealed record PreviewExchangeResponse(
    string FromCurrency,
    string ToCurrency,
    decimal SourceAmount,
    decimal TargetAmount,
    decimal Rate,
    DateTime RetrievedAtUtc);
