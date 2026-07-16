using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using FastChange.Application.Features.Users.RegisterUser;
using MediatR;
using Resources;

namespace Application.Features.Users.RegisterUser;

/// <summary>
/// Command handler for registering a new user. It checks for email uniqueness, hashes the password, creates default wallets, and persists the user and wallets to the database.
/// </summary>
public sealed class RegisterUserCommandHandler 
    : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;

    public RegisterUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var emailExists = await _userRepository.IsEmailTakenAsync(request.Email, cancellationToken);

        if (emailExists)
            throw new BusinessException(Localization.UserIsAlreadyExist);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            IsVerified = false
        };

        var defaultWallets = new List<Wallet>
        {
            new() { Id = Guid.NewGuid(), UserId = user.Id, Currency = "USD" },
            new() { Id = Guid.NewGuid(), UserId = user.Id, Currency = "BTC" }
        };

        await _userRepository.SaveUserWithWalletsAsync(user, defaultWallets, cancellationToken);

        return user.Id;
    }
}