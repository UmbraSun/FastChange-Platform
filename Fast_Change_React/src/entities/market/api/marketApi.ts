import { apiClient } from "@/shared/api/apiClient";
import type { MarketDataItem } from "../model/dto";

export async function getMarketOverview(
  currencies: string[],
  currency = "USD",
): Promise<MarketDataItem[]> {
  const response = await apiClient.get<MarketDataItem[]>(
    "/Portfolio/market",
    {
      params: {
        currencies,
        currency,
      },
      paramsSerializer: {
        indexes: null,
      },
    },
  );

  return response.data;
}