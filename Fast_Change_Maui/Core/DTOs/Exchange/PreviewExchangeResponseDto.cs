namespace Core.DTOs.Exchange;

public sealed record PreviewExchangeResponseDto(
    decimal ExchangeRate,
    decimal SentAmount,
    decimal ReceivedAmount);
