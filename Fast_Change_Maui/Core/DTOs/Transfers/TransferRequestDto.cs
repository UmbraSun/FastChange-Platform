namespace Core.DTOs.Transfers;

public sealed record TransferRequestDto(
    Guid FromWalletId,
    Guid ToWalletId,
    decimal Amount);