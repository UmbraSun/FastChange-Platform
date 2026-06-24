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

    public async Task<List<Transaction>> GetByWalletIdAsync(
        Guid walletId,
        CancellationToken cancellationToken)
    {
        return await _context.Transactions
            .Where(x => x.WalletId == walletId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }
}
