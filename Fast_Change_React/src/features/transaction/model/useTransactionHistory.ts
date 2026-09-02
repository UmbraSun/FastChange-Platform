import { useQueries } from "@tanstack/react-query";
import { useWallets } from "@/entities/wallet/model/useWallets";
import { getWalletHistory } from "@/entities/wallet/api/historyApi";
import type { WalletHistoryItem } from "@/entities/wallet/model/historyDto";

const HISTORY_TAKE = 50;

export interface TransactionHistoryItem extends WalletHistoryItem {
  walletId: string;
  currency: string;
}

export function useTransactionHistory() {
  const {
    data: wallets,
    isLoading: isWalletsLoading,
    isError: isWalletsError,
  } = useWallets();

  const historyQueries = useQueries({
    queries: (wallets ?? []).map((wallet) => ({
      queryKey: ["wallet-history", wallet.walletId],

      queryFn: () =>
        getWalletHistory(
          wallet.walletId,
          HISTORY_TAKE,
        ),

      enabled: Boolean(wallet.walletId),
    })),
  });

  const transactions: TransactionHistoryItem[] =
    historyQueries.flatMap((query, index) => {
      const wallet = wallets?.[index];

      if (!wallet) {
        return [];
      }

      return (query.data ?? []).map((transaction) => ({
        ...transaction,
        walletId: wallet.walletId,
        currency: wallet.currency,
      }));
    });

  transactions.sort(
    (a, b) =>
      new Date(b.createdAtUtc).getTime() -
      new Date(a.createdAtUtc).getTime(),
  );

  const isLoading =
    isWalletsLoading ||
    historyQueries.some((query) => query.isLoading);

  const isError =
    isWalletsError ||
    historyQueries.some((query) => query.isError);

  return {
    data: transactions,
    isLoading,
    isError,
  };
}
