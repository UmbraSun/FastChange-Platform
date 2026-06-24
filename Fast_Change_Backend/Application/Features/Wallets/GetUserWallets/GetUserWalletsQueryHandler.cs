using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Resources;

namespace Application.Features.Wallets.GetUserWallets;

public sealed class GetUserWalletsQueryHandler
    : IRequestHandler<GetUserWalletsQuery, List<GetUserWalletsResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUser;

    public GetUserWalletsQueryHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUser)
    {
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public async Task<List<GetUserWalletsResponse>> Handle(
        GetUserWalletsQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(
            _currentUser.UserId,
            cancellationToken);

        if (user is null)
            throw new BusinessException(Localization.UserNotFound);

        return user.Wallets
            .Select(x => new GetUserWalletsResponse(
                x.Id,
                x.Currency,
                x.Balance))
            .ToList();
    }
}