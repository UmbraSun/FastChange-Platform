import { useMutation, useQueryClient } from "@tanstack/react-query";
import { exchange } from "../api/exchange";

export function useExchange() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: exchange,

    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["wallets"],
      });
    },
  });
}