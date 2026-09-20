namespace Core.DTOs.Wallets;

public sealed record DepositResponseDto(
    Guid WalletId,
    decimal NewBalance);