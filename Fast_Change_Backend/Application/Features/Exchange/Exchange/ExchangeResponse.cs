namespace Application.Features.Exchange.Exchange;

public sealed record ExchangeResponse(
    decimal SentAmount,
    decimal ReceivedAmount,
    decimal Rate);
