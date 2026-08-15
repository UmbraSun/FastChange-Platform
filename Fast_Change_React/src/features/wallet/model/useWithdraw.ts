import { useMutation } from "@tanstack/react-query";
import { withdraw, } from "../api/withdraw";
import type { WithdrawRequest, } from "./dto";

export function useWithdraw() {
  return useMutation({
    mutationFn: (data: WithdrawRequest) =>
      withdraw(data),
  });
}