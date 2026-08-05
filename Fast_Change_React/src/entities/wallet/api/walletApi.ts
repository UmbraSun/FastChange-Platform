import { apiClient } from "@/shared/api/apiClient";
import type { WalletDto } from "../model/dto";

export async function getUserWallets(): Promise<WalletDto[]> {
  const response = await apiClient.get<WalletDto[]>(
    "/User/wallets"
  );
  return response.data;
}