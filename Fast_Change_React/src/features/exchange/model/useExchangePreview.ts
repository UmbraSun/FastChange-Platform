import { useQuery } from "@tanstack/react-query";
import { previewExchange } from "../api/preview";

interface Props {
  fromWalletId: string;
  toWalletId: string;
  amount: number;
}

export function useExchangePreview({
  fromWalletId,
  toWalletId,
  amount,
}: Props) {
  return useQuery({
    queryKey: [
      "exchange-preview",
      fromWalletId,
      toWalletId,
      amount,
    ],

    queryFn: () =>
      previewExchange({
        fromWalletId,
        toWalletId,
        amount,
      }),

    enabled:
      Boolean(fromWalletId) &&
      Boolean(toWalletId) &&
      amount > 0,
  });
}