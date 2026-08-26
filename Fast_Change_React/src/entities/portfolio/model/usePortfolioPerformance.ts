import { useQuery } from "@tanstack/react-query";
import { getPortfolioPerformance } from "../api/portfolioApi";

export function usePortfolioPerformance(
  currency = "USD",
) {
  return useQuery({
    queryKey: [
      "portfolio-performance",
      currency,
    ],
    queryFn: () =>
      getPortfolioPerformance(currency),
  });
}