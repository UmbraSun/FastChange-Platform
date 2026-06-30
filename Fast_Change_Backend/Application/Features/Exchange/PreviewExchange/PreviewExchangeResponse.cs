namespace Application.Features.Exchange.PreviewExchange;

public sealed record PreviewExchangeResponse(
    decimal ExchangeRate,
    decimal SentAmount,
    decimal ReceivedAmount);
