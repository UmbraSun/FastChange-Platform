import { apiClient } from "@/shared/api/apiClient";
import type { TransferRecipient } from "../model/dto";

export async function searchTransferRecipients(
  query: string,
  currency: string,
): Promise<TransferRecipient[]> {
  const response = await apiClient.get<TransferRecipient[]>(
    "/transfers/recipients",
    {
      params: {
        query,
        currency,
      },
    },
  );

  return response.data;
}
