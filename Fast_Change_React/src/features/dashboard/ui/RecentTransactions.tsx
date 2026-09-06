import { ArrowDownToLine, ArrowLeftRight, ArrowUpFromLine, Send, } from "lucide-react";
import { useRecentTransactions } from "@/features/transaction/model/useRecentTransactions";

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

function getOperationType(
  operationType: string,
) {
  return operationType
    .trim()
    .toLowerCase();
}

function getOperationLabel(
  operationType: string,
) {
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

function getOperationIcon(
  operationType: string,
) {
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

export function RecentTransactions() {
  const {
    data: transactions,
    isLoading,
    isError,
  } = useRecentTransactions();

  return (
    <section className="rounded-3xl border border-exchange-border bg-exchange-card p-5">
      <h2 className="mb-4 text-lg font-semibold">
        Recent activity
      </h2>

      {isLoading ? (
        <div className="space-y-3">
          {[1, 2, 3].map((item) => (
            <div
              key={item}
              className="flex items-center justify-between rounded-2xl bg-black/10 p-3"
            >
              <div className="flex items-center gap-3">
                <div className="h-10 w-10 animate-pulse rounded-full bg-white/5" />

                <div className="space-y-2">
                  <div className="h-4 w-20 animate-pulse rounded bg-white/5" />
                  <div className="h-3 w-28 animate-pulse rounded bg-white/5" />
                </div>
              </div>

              <div className="space-y-2 text-right">
                <div className="ml-auto h-4 w-20 animate-pulse rounded bg-white/5" />
                <div className="ml-auto h-3 w-24 animate-pulse rounded bg-white/5" />
              </div>
            </div>
          ))}
        </div>
      ) : isError ? (
        <div className="rounded-2xl bg-black/10 p-4 text-sm text-exchange-muted">
          Failed to load recent activity.
        </div>
      ) : transactions.length === 0 ? (
        <div className="rounded-2xl bg-black/10 p-4 text-sm text-exchange-muted">
          No recent activity.
        </div>
      ) : (
        <div className="space-y-3">
          {transactions.map((transaction) => {
            const isPositive =
              transaction.signedAmount > 0;

            const Icon = getOperationIcon(
              transaction.operationType,
            );

            return (
              <div
                key={`${transaction.walletId}-${transaction.operationId}`}
                className="
                  flex
                  items-center
                  justify-between
                  gap-4
                  rounded-2xl
                  bg-black/10
                  p-3
                "
              >
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

                    <p className="truncate text-xs text-exchange-muted">
                      {transaction.currency}
                      {" · "}
                      {new Date(
                        transaction.createdAtUtc,
                      ).toLocaleString()}
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

                  <p className="text-xs text-exchange-muted">
                    Balance{" "}
                    {formatAmount(
                      transaction.balanceAfter,
                      transaction.currency,
                    )}
                  </p>
                </div>
              </div>
            );
          })}
        </div>
      )}
    </section>
  );
}
