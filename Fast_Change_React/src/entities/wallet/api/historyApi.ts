import { apiClient } from "@/shared/api/apiClient";
import type { WalletHistoryItem } from "../model/historyDto";

export async function getWalletHistory(
  walletId: string,
  take = 50,
): Promise<WalletHistoryItem[]> {
  const response =
    await apiClient.get<WalletHistoryItem[]>(
      `/Wallet/${walletId}/history`,
      {
        params: {
          take,
        },
      },
    );

  return response.data;
}