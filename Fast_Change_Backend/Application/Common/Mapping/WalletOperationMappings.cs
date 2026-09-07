using Application.Features.Wallets.Deposit;
using Application.Features.Wallets.Withdraw;
using Domain.Entities;
using Mapster;

namespace Application.Common.Mapping;

public sealed class WalletOperationMappings
    : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Wallet, DepositResponse>()
            .Map(dest => dest.WalletId, src => src.Id)
            .Map(dest => dest.NewBalance, src => src.Balance);

        config.NewConfig<Wallet, WithdrawResponse>()
            .Map(dest => dest.WalletId, src => src.Id)
            .Map(dest => dest.Balance, src => src.Balance);
    }
}
