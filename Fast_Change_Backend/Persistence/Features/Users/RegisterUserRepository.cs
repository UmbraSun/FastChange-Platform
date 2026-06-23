using Application.Features.Users.RegisterUser;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Features.Users;

public sealed class RegisterUserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public RegisterUserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetByIdAsync(Guid Id, CancellationToken cancellationToken)
        => await _context.Users.Include(x => x.Wallets).FirstOrDefaultAsync(u => u.Id == Id, cancellationToken);

    public async Task<bool> IsEmailTakenAsync(string email, CancellationToken cancellationToken)
        => await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);

    public async Task SaveUserWithWalletsAsync(User user, List<Wallet> wallets, CancellationToken cancellationToken)
    {
        _context.Users.Add(user);
        _context.Wallets.AddRange(wallets);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
