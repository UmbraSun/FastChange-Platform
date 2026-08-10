import { apiClient } from "@/shared/api/apiClient";
import type { WithdrawRequest, WithdrawResponse, } from "../model/dto";

export async function withdraw(
  data: WithdrawRequest
): Promise<WithdrawResponse> {
  const response =
    await apiClient.post<WithdrawResponse>(
      "/Wallet/withdraw",
      data
    );

  return response.data;
}