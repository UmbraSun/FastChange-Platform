import { useQuery } from "@tanstack/react-query";
import { searchTransferRecipients } from "../api/recipients";

export function useTransferRecipients(
  query: string,
  currency: string,
) {
  return useQuery({
    queryKey: [
      "transfer-recipients",
      query,
      currency,
    ],

    queryFn: () =>
      searchTransferRecipients(
        query,
        currency,
      ),

    enabled:
      query.trim().length > 0 &&
      Boolean(currency),

    staleTime: 30_000,
  });
}
