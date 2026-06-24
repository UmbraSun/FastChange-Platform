using Application.Common.Interfaces;
using Domain.Entities;

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
}
