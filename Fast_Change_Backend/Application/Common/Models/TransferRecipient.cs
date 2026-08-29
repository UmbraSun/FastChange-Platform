namespace Application.Common.Models;

/// <summary>
/// Transfer recipient information.
/// </summary>
public sealed record TransferRecipient(
    Guid UserId,
    string Email,
    Guid WalletId,
    string Currency);
