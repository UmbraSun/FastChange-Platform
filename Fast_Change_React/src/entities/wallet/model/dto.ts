export interface WalletDto {
  walletId: string;
  currency: string;
  balance: number;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}