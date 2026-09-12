namespace Core.DTOs.Exchange;

public sealed record ExchangeRequestDto(
    Guid FromWalletId,
    Guid ToWalletId,
    decimal Amount);
