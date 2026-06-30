namespace Application.Features.Exchange.ExchangeCurrency;

public sealed record ExchangeResponse(
    decimal ExchangeRate,
    decimal SentAmount,
    decimal ReceivedAmount,
    decimal SourceBalance,
    decimal DestinationBalance);
