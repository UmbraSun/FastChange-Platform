using MediatR;

namespace Application.Features.Wallets.GetUserWallets;

public sealed record GetUserWalletsQuery : IRequest<List<GetUserWalletsResponse>>;
