import { apiClient } from "@/shared/api/apiClient";
import type {
  ExchangeRequest,
  ExchangePreviewResponse,
  ExchangeResponse,
} from "../model/dto";


export async function getExchangePreview(
  data: ExchangeRequest
): Promise<ExchangePreviewResponse> {
  const response = await apiClient.get<ExchangePreviewResponse>(
    "/exchange/preview",
    {
      params: data,
    }
  );

  return response.data;
}

export async function exchangeCurrency(
  data: ExchangeRequest
): Promise<ExchangeResponse> {
  const response = await apiClient.post<ExchangeResponse>(
    "/exchange",
    data
  );

  return response.data;
}