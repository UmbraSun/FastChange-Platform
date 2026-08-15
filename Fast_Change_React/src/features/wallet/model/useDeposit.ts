import { useMutation } from "@tanstack/react-query";
import { deposit, type DepositRequest, } from "../api/deposit";

export function useDeposit() {
  return useMutation({
    mutationFn: (data: DepositRequest) =>
      deposit(data),
  });
}
