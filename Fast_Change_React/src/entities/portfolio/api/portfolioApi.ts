import { apiClient } from "@/shared/api/apiClient";
import type { PortfolioPerformanceResponse, } from "../model/dto";

export async function getPortfolioPerformance(
  currency = "USD",
): Promise<PortfolioPerformanceResponse> {
  const response =
    await apiClient.get<PortfolioPerformanceResponse>(
      "/Portfolio/performance",
      {
        params: {
          currency,
        },
      },
    );

  return response.data;
}