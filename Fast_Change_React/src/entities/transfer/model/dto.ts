export interface TransferRecipient {
  userId: string;
  email: string;
  walletId: string;
  currency: string;
}

export interface SearchTransferRecipientsRequest {
  query: string;
  currency: string;
}

export interface TransferRequest {
  fromWalletId: string;
  toWalletId: string;
  amount: number;
}

export interface TransferResponse {
  operationId: string;
  amount: number;
  senderBalance: number;
  receiverBalance: number;
}