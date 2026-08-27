import { useQuery } from "@tanstack/react-query";
import { getMarketOverview } from "../api/marketApi";

const MARKET_CURRENCIES = [
  "BTC",
  "ETH",
  "SOL",
];

export function useMarketOverview(
  currency = "USD",
) {
  return useQuery({
    queryKey: [
      "market-overview",
      MARKET_CURRENCIES,
      currency,
    ],

    queryFn: () =>
      getMarketOverview(
        MARKET_CURRENCIES,
        currency,
      ),
  });
}