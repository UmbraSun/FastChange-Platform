import { useTransactionHistory } from "./useTransactionHistory";

const RECENT_TRANSACTIONS_COUNT = 5;

export function useRecentTransactions() {
  const {
    data,
    isLoading,
    isError,
  } = useTransactionHistory();

  return {
    data: data.slice(0, RECENT_TRANSACTIONS_COUNT),
    isLoading,
    isError,
  };
}
