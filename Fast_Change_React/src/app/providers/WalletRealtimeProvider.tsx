import { useCallback } from "react";
import { useQueryClient } from "@tanstack/react-query";
import { useWalletHub } from "@/shared/api/signalr/useWalletHub";

export function WalletRealtimeProvider() {
  const queryClient = useQueryClient();

  const handleWalletUpdated = useCallback(
    (walletId: string) => {
      void queryClient.invalidateQueries({
        queryKey: [
          "wallet-history",
          walletId,
        ],
      });

      void queryClient.invalidateQueries({
        queryKey: ["user-wallets"],
      });

      void queryClient.invalidateQueries({
        queryKey: ["portfolio-performance"],
      });
    },
    [queryClient],
  );

  useWalletHub(handleWalletUpdated);

  return null;
}