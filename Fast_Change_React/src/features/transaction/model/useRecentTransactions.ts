import { useQueries } from "@tanstack/react-query";
import { useWallets } from "@/entities/wallet/model/useWallets";
import { getWalletHistory } from "@/entities/wallet/api/historyApi";
import type { WalletHistoryItem } from "@/entities/wallet/model/historyDto";

const RECENT_TRANSACTIONS_COUNT = 5;
const HISTORY_TAKE = 50;

export function useRecentTransactions() {
  const {
    data: wallets,
    isLoading: isWalletsLoading,
  } = useWallets();

  const historyQueries = useQueries({
    queries: (wallets ?? []).map((wallet) => ({
      queryKey: [
        "wallet-history",
        wallet.walletId,
      ],

      queryFn: () =>
        getWalletHistory(
          wallet.walletId,
          HISTORY_TAKE,
        ),

      enabled: Boolean(wallet.walletId),
    })),
  });

  const transactions: WalletHistoryItem[] =
    historyQueries
      .flatMap(
        (query) => query.data ?? [],
      )
      .sort(
        (a, b) =>
          new Date(b.createdAtUtc).getTime() -
          new Date(a.createdAtUtc).getTime(),
      )
      .slice(
        0,
        RECENT_TRANSACTIONS_COUNT,
      );

  const isLoading = isWalletsLoading ||
    historyQueries.some(
      (query) => query.isLoading,
    );

  console.log(
    "RECENT TRANSACTIONS",
    historyQueries.map(query => query.data),
  );

  return {
    data: transactions,
    isLoading,
  };
}