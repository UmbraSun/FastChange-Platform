using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Portfolio.GetPortfolio;
using Domain.Entities;
using Mapster;

namespace Application.Common.Mapping;

public sealed class PortfolioMappings
    : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Wallet, PortfolioWalletResponse>()
            .Map(dest => dest.WalletId, src => src.Id)
            .Map(dest => dest.ExchangeRate, src => 1m)
            .Map(dest => dest.ConvertedValue, src => src.Balance);
    }
}
