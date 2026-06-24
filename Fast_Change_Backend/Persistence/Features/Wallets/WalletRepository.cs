using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Features.Wallets;

public sealed class WalletRepository : IWalletRepository
{
    private readonly ApplicationDbContext _context;

    public WalletRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Wallet?> GetByIdAsync(
        Guid walletId,
        CancellationToken cancellationToken)
    {
        return await _context.Wallets
            .FirstOrDefaultAsync(
                w => w.Id == walletId,
                cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
