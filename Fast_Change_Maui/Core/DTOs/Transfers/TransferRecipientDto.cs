namespace Core.DTOs.Transfers;

public sealed record TransferRecipientDto(
    Guid UserId,
    string Email,
    Guid WalletId,
    string Currency);