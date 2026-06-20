using MediatR;

namespace FastChange.Application.Features.Users.RegisterUser;

public sealed record RegisterUserCommand(
    string Email,
    string Password) : IRequest<Guid>; // Returns the generated User ID on success