export interface WalletHistoryItem {
  operationId: string;
  signedAmount: number;
  balanceAfter: number;
  operationType: string;
  exchangeRate: number | null;
  receivedAmount: number | null;
  createdAtUtc: string;
}