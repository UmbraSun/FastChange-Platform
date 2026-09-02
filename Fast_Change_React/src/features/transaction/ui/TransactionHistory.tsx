import {
  ArrowDownToLine,
  ArrowLeftRight,
  ArrowUpFromLine,
  Send,
} from "lucide-react";

import type { TransactionHistoryItem } from "../model/useTransactionHistory";

interface TransactionHistoryProps {
  transactions: TransactionHistoryItem[];
}

interface TransactionGroup {
  key: string;
  label: string;
  transactions: TransactionHistoryItem[];
}

function getOperationType(operationType: string) {
  return operationType.trim().toLowerCase();
}

function getOperationLabel(operationType: string) {
  switch (getOperationType(operationType)) {
    case "deposit":
      return "Deposit";

    case "withdraw":
      return "Withdraw";

    case "exchange":
      return "Exchange";

    case "transfer":
      return "Transfer";

    default:
      return operationType;
  }
}

function getOperationIcon(operationType: string) {
  switch (getOperationType(operationType)) {
    case "deposit":
      return ArrowDownToLine;

    case "withdraw":
      return ArrowUpFromLine;

    case "transfer":
      return Send;

    case "exchange":
    default:
      return ArrowLeftRight;
  }
}

function getCurrencyDecimals(currency: string) {
  return currency === "BTC" ? 8 : 2;
}

function formatAmount(
  amount: number,
  currency: string,
) {
  return amount.toFixed(
    getCurrencyDecimals(currency),
  );
}

function formatDate(date: string) {
  return new Date(date).toLocaleDateString(
    undefined,
    {
      month: "short",
      day: "numeric",
      year: "numeric",
    },
  );
}

function formatTime(date: string) {
  return new Date(date).toLocaleTimeString(
    undefined,
    {
      hour: "2-digit",
      minute: "2-digit",
    },
  );
}

function groupTransactionsByDate(
  transactions: TransactionHistoryItem[],
): TransactionGroup[] {
  const groups = new Map<
    string,
    TransactionGroup
  >();

  for (const transaction of transactions) {
    const date = new Date(
      transaction.createdAtUtc,
    );

    const key = [
      date.getFullYear(),
      date.getMonth(),
      date.getDate(),
    ].join("-");

    const existing = groups.get(key);

    if (existing) {
      existing.transactions.push(transaction);
      continue;
    }

    groups.set(key, {
      key,
      label: formatDate(
        transaction.createdAtUtc,
      ),
      transactions: [transaction],
    });
  }

  return Array.from(groups.values());
}

function formatReceivedAmount(
  amount: number,
) {
  return Number.isInteger(amount)
    ? amount.toString()
    : amount.toFixed(8).replace(/0+$/, "").replace(/\.$/, "");
}

export function TransactionHistory({
  transactions,
}: TransactionHistoryProps) {
  const groups =
    groupTransactionsByDate(transactions);

  return (
    <section className="space-y-7">
      {groups.map((group) => (
        <div
          key={group.key}
          className="space-y-3"
        >
          <div className="flex items-center justify-between px-1">
            <h2
              className="
                text-xs
                font-semibold
                uppercase
                tracking-wider
                text-exchange-muted
              "
            >
              {group.label}
            </h2>

            <span className="text-xs text-exchange-muted">
              {group.transactions.length}
            </span>
          </div>

          <div
            className="
              overflow-hidden
              rounded-3xl
              border
              border-exchange-border
              bg-exchange-card
            "
          >
            {group.transactions.map(
              (transaction, index) => {
                const isPositive =
                  transaction.signedAmount > 0;

                const operationType =
                  getOperationType(
                    transaction.operationType,
                  );

                const Icon =
                  getOperationIcon(
                    transaction.operationType,
                  );

                const isLast =
                  index ===
                  group.transactions.length - 1;

                return (
                  <article
                    key={`${transaction.walletId}-${transaction.operationId}`}
                    className={`
                      p-4
                      transition-colors
                      hover:bg-white/[0.02]
                      ${
                        !isLast
                          ? "border-b border-exchange-border"
                          : ""
                      }
                    `}
                  >
                    <div className="flex items-center justify-between gap-4">
                      <div className="flex min-w-0 items-center gap-3">
                        <div
                          className="
                            flex
                            h-10
                            w-10
                            shrink-0
                            items-center
                            justify-center
                            rounded-full
                            bg-black/20
                          "
                        >
                          <Icon className="h-5 w-5 text-exchange-gold" />
                        </div>

                        <div className="min-w-0">
                          <p className="font-medium">
                            {getOperationLabel(
                              transaction.operationType,
                            )}
                          </p>

                          <p className="mt-0.5 text-xs text-exchange-muted">
                            {transaction.currency}
                            {" · "}
                            {formatTime(
                              transaction.createdAtUtc,
                            )}
                          </p>
                        </div>
                      </div>

                      <div className="shrink-0 text-right">
                        <p
                          className={
                            isPositive
                              ? "font-semibold text-green-400"
                              : "font-semibold text-red-400"
                          }
                        >
                          {isPositive ? "+" : ""}
                          {formatAmount(
                            transaction.signedAmount,
                            transaction.currency,
                          )}{" "}
                          {transaction.currency}
                        </p>

                        <p className="mt-0.5 text-xs text-exchange-muted">
                          Balance{" "}
                          {formatAmount(
                            transaction.balanceAfter,
                            transaction.currency,
                          )}
                        </p>
                      </div>
                    </div>

                    {operationType ===
                      "exchange" &&
                      (transaction.exchangeRate !==
                        null ||
                        transaction.receivedAmount !==
                          null) && (
                        <div
                          className="
                            mt-3
                            rounded-2xl
                            bg-black/10
                            px-3
                            py-2.5
                            text-xs
                          "
                        >
                          {transaction.exchangeRate !==
                            null && (
                            <div className="flex justify-between gap-4">
                              <span className="text-exchange-muted">
                                Rate
                              </span>

                              <span>
                                {
                                  transaction.exchangeRate
                                }
                              </span>
                            </div>
                          )}

                          {transaction.receivedAmount !==
                            null && (
                            <div className="mt-1.5 flex justify-between gap-4">
                              <span className="text-exchange-muted">
                                Received
                              </span>

                              <span className="font-medium">
                                +
                                {formatReceivedAmount(
                                  transaction.receivedAmount,
                                )}
                              </span>
                            </div>
                          )}
                        </div>
                      )}
                  </article>
                );
              },
            )}
          </div>
        </div>
      ))}
    </section>
  );
}
