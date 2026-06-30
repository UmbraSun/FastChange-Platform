using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Features.Transactions;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _context;

    public TransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Transaction transaction,
        CancellationToken cancellationToken)
    {
        await _context.Transactions.AddAsync(
            transaction,
            cancellationToken);
    }

    public async Task AddRangeAsync(
        IEnumerable<Transaction> transactions,
        CancellationToken cancellationToken)
    {
        await _context.Transactions.AddRangeAsync(
            transactions,
            cancellationToken);
    }

    public async Task<(List<Transaction> Items, int TotalCount)> GetByWalletIdAsync(
        Guid walletId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = _context.Transactions
            .Where(x => x.WalletId == walletId);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
