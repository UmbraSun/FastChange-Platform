namespace Core.DTOs.Wallets;

public sealed record WithdrawRequestDto(
    Guid WalletId,
    decimal Amount);