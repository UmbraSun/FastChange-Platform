import { ArrowDownToLine, ArrowLeftRight, ArrowUpFromLine } from "lucide-react";
import { useRecentTransactions } from "@/features/transaction/model/useRecentTransactions";

export function RecentTransactions() {
  const {
    data: transactions,
    isLoading,
  } = useRecentTransactions();

  return (
    <section className="rounded-3xl border border-exchange-border bg-exchange-card p-5">
      <h2 className="mb-4 text-lg font-semibold">
        Recent activity
      </h2>

      <div className="space-y-3">
        {isLoading ? (
          <div className="text-sm text-exchange-muted">
            Loading activity...
          </div>
        ) : transactions.length === 0 ? (
          <div className="text-sm text-exchange-muted">
            No recent activity
          </div>
        ) : (
          transactions.map((transaction) => {
            const isPositive = transaction.signedAmount > 0;
            const isDeposit = transaction.operationType.toLowerCase() === "deposit";
            const isWithdraw = transaction.operationType.toLowerCase() === "withdraw";
            const Icon = isDeposit
              ? ArrowDownToLine
              : isWithdraw
                ? ArrowUpFromLine
                : ArrowLeftRight;

            return (
              <div
                key={transaction.operationId}
                className="
                  flex
                  items-center
                  justify-between
                  rounded-2xl
                  bg-black/10
                  p-3
                "
              >
                <div className="flex items-center gap-3">
                  <div
                    className="
                      flex
                      h-10
                      w-10
                      items-center
                      justify-center
                      rounded-full
                      bg-black/20
                    "
                  >
                    <Icon className="h-5 w-5 text-exchange-gold" />
                  </div>

                  <div>
                    <p className="font-medium">
                      {transaction.operationType}
                    </p>

                    <p className="text-xs text-exchange-muted">
                      {new Date(
                        transaction.createdAtUtc,
                      ).toLocaleString()}
                    </p>
                  </div>
                </div>

                <div className="text-right">
                  <p
                    className={
                      isPositive
                        ? "font-semibold text-green-400"
                        : "font-semibold text-red-400"
                    }
                  >
                    {isPositive ? "+" : ""}
                    {transaction.signedAmount}
                  </p>

                  <p className="text-xs text-exchange-muted">
                    Balance: {transaction.balanceAfter}
                  </p>
                </div>
              </div>
            );
          })
        )}
      </div>
    </section>
  );
}
