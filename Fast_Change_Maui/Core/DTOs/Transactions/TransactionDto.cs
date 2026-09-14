namespace Core.DTOs.Transactions;

public sealed record TransactionDto(
    Guid TransactionId,
    string Currency,
    decimal Amount,
    string Type,
    DateTime CreatedAtUtc);
