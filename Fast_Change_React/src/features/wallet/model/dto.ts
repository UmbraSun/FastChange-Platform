export interface WalletResponse {
  walletId: string;
  currency: string;
  balance: number;
}

export interface DepositRequest {
  walletId: string;
  amount: number;
}

export interface DepositResponse {
  walletId: string;
  newBalance: number;
}

export interface WithdrawRequest {
  walletId: string;
  amount: number;
}

export interface WithdrawResponse {
  walletId: string;
  balance: number;
}