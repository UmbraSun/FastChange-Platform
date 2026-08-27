export interface MarketDataItem {
  currency: string;
  quoteCurrency: string;
  price: number;
  priceChangePercentage24h: number | null;
}