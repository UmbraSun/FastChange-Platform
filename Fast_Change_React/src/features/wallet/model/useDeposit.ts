import { useMutation, useQueryClient } from "@tanstack/react-query";
import { deposit, type DepositRequest, } from "../api/deposit";

export function useDeposit() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: DepositRequest) => deposit(data),

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
