import { useQuery, } from "@tanstack/react-query";
import { getExchangePreview, } from "../api/exchangeApi";
import type { ExchangeRequest, } from "./dto";

export function useExchangePreview(
  data: ExchangeRequest
) {
  return useQuery({
    queryKey: [
      "exchange-preview",
      data,
    ],
    queryFn: () => getExchangePreview(data),
    enabled:
      Boolean(data.fromWalletId) &&
      Boolean(data.toWalletId) &&
      data.amount > 0,
  });
}