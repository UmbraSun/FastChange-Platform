import { useMutation, } from "@tanstack/react-query";
import { exchangeCurrency, } from "../api/exchangeApi";

export function useExchange() {
  return useMutation({
    mutationFn:
      exchangeCurrency,
  });
}