import { apiClient } from "@/shared/api/apiClient";
import type { WalletResponse, } from "../model/dto";

export async function getUserWallets(): Promise<WalletResponse[]> {
  const response =
    await apiClient.get<WalletResponse[]>(
      "/User/wallets"
    );

  return response.data;
}