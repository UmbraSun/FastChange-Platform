export interface ExchangeRequest {
  fromWalletId: string;
  toWalletId: string;
  amount: number;
}

export interface ExchangeResponse {
  exchangeRate: number;
  sentAmount: number;
  receivedAmount: number;
  sourceBalance: number;
  destinationBalance: number;
}

export interface ExchangePreviewRequest {
  fromWalletId: string;
  toWalletId: string;
  amount: number;
}

export interface ExchangePreviewResponse {
  exchangeRate: number;
  sentAmount: number;
  receivedAmount: number;
}