using Domain.Entities;

namespace Application.Common.Interfaces;

/// <summary>
/// Represents a repository for managing transactions in the application.
/// </summary>
public interface ITransactionRepository
{
    /// <summary>
    /// Add transaction to the database
    /// </summary>
    /// <param name="transaction"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task AddAsync(
        Transaction transaction,
        CancellationToken cancellationToken);
}
