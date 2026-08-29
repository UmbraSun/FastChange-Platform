import { apiClient } from "@/shared/api/apiClient";
import type { TransferRequest, TransferResponse, } from "../model/dto";

export async function transfer(
  data: TransferRequest,
): Promise<TransferResponse> {
  const response = await apiClient.post<TransferResponse>(
    "/transfers",
    data,
  );

  return response.data;
}
