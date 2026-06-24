using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Features.Auth;

public sealed class LoginUserRepository : ILoginUserRepository
{
    private readonly ApplicationDbContext _context;

    public LoginUserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
}
