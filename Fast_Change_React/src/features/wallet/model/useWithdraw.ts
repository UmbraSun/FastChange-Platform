import { useMutation, useQueryClient } from "@tanstack/react-query";
import { withdraw, } from "../api/withdraw";
import type { WithdrawRequest, } from "./dto";

export function useWithdraw() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: WithdrawRequest) => withdraw(data),

    onSuccess: async (_, variables) => {
      await Promise.all([
        queryClient.invalidateQueries({
          queryKey: ["user-wallets"],
        }),

        queryClient.invalidateQueries({
          queryKey: [
            "wallet-history",
            variables.walletId,
          ],
        }),
      ]);
    },
  });
}