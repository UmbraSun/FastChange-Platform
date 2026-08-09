import { apiClient } from "@/shared/api/apiClient";
import type {
  ExchangeRequest,
  ExchangeResponse,
} from "../model/dto";

export async function exchange(
  data: ExchangeRequest
): Promise<ExchangeResponse> {

  const response = await apiClient.post(
    "/exchange",
    data
  );

  return response.data;
}