import { apiClient } from "@/shared/api/apiClient";

export interface DepositRequest {
  walletId: string;
  amount: number;
}

export interface DepositResponse {
  walletId: string;
  newBalance: number;
}

export async function deposit(
  data: DepositRequest
): Promise<DepositResponse> {
  const response = await apiClient.post<DepositResponse>(
    "/Wallet/deposit",
    data
  );

  return response.data;
}
