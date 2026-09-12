namespace Core.DTOs.Exchange;

public sealed record ExchangeResponseDto(
    decimal ExchangeRate,
    decimal SentAmount,
    decimal ReceivedAmount,
    decimal SourceBalance,
    decimal DestinationBalance);
