namespace Core.DTOs.Wallets;

public sealed record WithdrawResponseDto(
    Guid WalletId,
    decimal Balance);