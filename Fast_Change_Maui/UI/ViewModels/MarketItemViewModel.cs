using CommunityToolkit.Mvvm.ComponentModel;
using Core.DTOs.Portfolio;

namespace UI.ViewModels;

public sealed partial class MarketItemViewModel : ObservableObject
{
    public string Pair => $"{Currency}/{QuoteCurrency}";

    public string Currency { get; }

    public string QuoteCurrency { get; }

    public decimal Price { get; }

    public decimal? PriceChangePercentage24h { get; }

    public string DisplayPrice => $"{Price:N2} {QuoteCurrency}";

    public string DisplayChange => PriceChangePercentage24h is null ? "—" : $"{(PriceChangePercentage24h > 0 ? "+" : string.Empty)}{PriceChangePercentage24h.Value:0.00}%";

    public bool IsPositive => PriceChangePercentage24h > 0;

    public bool IsNegative => PriceChangePercentage24h < 0;

    public bool IsNeutral => PriceChangePercentage24h is null || PriceChangePercentage24h == 0;

    public MarketItemViewModel(MarketDataItemDto market)
    {
        Currency = market.Currency;
        QuoteCurrency = market.QuoteCurrency;
        Price = market.Price;
        PriceChangePercentage24h = market.PriceChangePercentage24h;
    }
}