namespace Core.DTOs.Wallets;

public sealed record WalletDto(
    Guid WalletId,
    string Currency,
    decimal Balance);
