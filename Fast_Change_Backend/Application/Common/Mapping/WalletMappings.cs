using Application.Features.Wallets.GetUserWallets;
using Domain.Entities;
using Mapster;

namespace Application.Common.Mapping;

public sealed class WalletMappings
    : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Wallet, GetUserWalletsResponse>()
            .Map(dest => dest.WalletId, src => src.Id);
    }
}
