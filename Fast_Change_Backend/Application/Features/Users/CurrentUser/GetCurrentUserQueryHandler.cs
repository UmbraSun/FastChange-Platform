using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Resources;

namespace Application.Features.Users.CurrentUser;

/// <summary>
/// Handler for retrieving the currently authenticated user's information.
/// </summary>
public sealed class GetCurrentUserQueryHandler 
    : IRequestHandler<GetCurrentUserQuery, GetCurrentUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUser;

    public GetCurrentUserQueryHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUser)
    {
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public async Task<GetCurrentUserResponse> Handle(
        GetCurrentUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .GetByIdAsync(_currentUser.UserId, cancellationToken);

        if (user is null)
            throw new BusinessException(Localization.UserNotFound);

        return new GetCurrentUserResponse(
            user.Id,
            user.Email,
            user.IsVerified);
    }
}