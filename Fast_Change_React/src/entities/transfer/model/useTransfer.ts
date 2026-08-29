import { useMutation, useQueryClient, } from "@tanstack/react-query";
import { toast } from "sonner";
import { transfer } from "../api/transfer";

export function useTransfer() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: transfer,

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
      ]);

      toast.success("Transfer completed");
    },

    onError: () => {
      toast.error("Transfer failed");
    },
  });
}