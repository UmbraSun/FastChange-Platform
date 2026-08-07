import { useQuery, } from "@tanstack/react-query";
import { getUserWallets, } from "../api/walletApi";

export function useWallets() {
  return useQuery({
    queryKey: [
      "wallets",
    ],
    queryFn: getUserWallets,
  });
}