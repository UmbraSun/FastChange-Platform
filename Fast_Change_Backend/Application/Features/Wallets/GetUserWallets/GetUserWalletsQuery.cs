using MediatR;

namespace Application.Features.Wallets.GetUserWallets;

public class GetUserWalletsQuery : IRequest<List<GetUserWalletsResponse>>;
