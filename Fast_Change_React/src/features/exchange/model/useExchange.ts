import { useMutation, useQueryClient } from "@tanstack/react-query";
import { exchange } from "../api/exchange";

export function useExchange() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: exchange,

    onSuccess: async (_, variables) => {
      await Promise.all([
        queryClient.invalidateQueries({
          queryKey: ["user-wallets"],
        }),

        queryClient.invalidateQueries({
          queryKey: [
            "wallet-history",
            variables.fromWalletId,
          ],
        }),

        queryClient.invalidateQueries({
          queryKey: [
            "wallet-history",
            variables.toWalletId,
          ],
        }),
      ]);
    },
  });
}