import { apiClient } from "@/shared/api/apiClient";
import type { ExchangePreviewRequest, ExchangePreviewResponse, } from "../model/dto";

export async function previewExchange(
  request: ExchangePreviewRequest,
) {
  const response =
    await apiClient.get<ExchangePreviewResponse>(
      "/exchange/preview",
      {
        params: request,
      },
    );

    return response.data;
}