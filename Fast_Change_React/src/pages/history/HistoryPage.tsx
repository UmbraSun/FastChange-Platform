import { useMemo, useState } from "react";
import { Filter } from "lucide-react";

import { TransactionHistory } from "@/features/transaction/ui/TransactionHistory";
import { useTransactionHistory } from "@/features/transaction/model/useTransactionHistory";

const ALL_WALLETS = "all";
const ALL_TYPES = "all";

export default function HistoryPage() {
  const {
    data: transactions,
    isLoading,
    isError,
  } = useTransactionHistory();

  const [selectedWallet, setSelectedWallet] =
    useState(ALL_WALLETS);

  const [selectedType, setSelectedType] =
    useState(ALL_TYPES);

  const currencies = useMemo(() => {
    return Array.from(
      new Set(
        transactions.map(
          (transaction) => transaction.currency,
        ),
      ),
    ).sort();
  }, [transactions]);

  const operationTypes = useMemo(() => {
    return Array.from(
      new Set(
        transactions.map((transaction) =>
          transaction.operationType.trim().toLowerCase(),
        ),
      ),
    ).sort();
  }, [transactions]);

  const filteredTransactions = useMemo(() => {
    return transactions.filter((transaction) => {
      const walletMatches =
        selectedWallet === ALL_WALLETS ||
        transaction.currency === selectedWallet;

      const typeMatches =
        selectedType === ALL_TYPES ||
        transaction.operationType.trim().toLowerCase() ===
          selectedType;

      return walletMatches && typeMatches;
    });
  }, [
    transactions,
    selectedWallet,
    selectedType,
  ]);

  const hasActiveFilters =
    selectedWallet !== ALL_WALLETS ||
    selectedType !== ALL_TYPES;

  function resetFilters() {
    setSelectedWallet(ALL_WALLETS);
    setSelectedType(ALL_TYPES);
  }

  return (
    <div className="mx-auto w-full max-w-3xl space-y-7">
      <header className="px-1">
        <div className="flex items-end justify-between gap-4">
          <div>
            <h1 className="text-2xl font-semibold tracking-tight">
              History
            </h1>

            <p className="mt-1 text-sm text-exchange-muted">
              Your recent account activity
            </p>
          </div>

          {!isLoading &&
            !isError &&
            transactions.length > 0 && (
              <span className="pb-0.5 text-xs text-exchange-muted">
                {filteredTransactions.length}{" "}
                {filteredTransactions.length === 1
                  ? "operation"
                  : "operations"}
              </span>
            )}
        </div>
      </header>

      {!isLoading &&
        !isError &&
        transactions.length > 0 && (
          <section
            className="
              rounded-3xl
              border
              border-exchange-border
              bg-exchange-card
              p-4
            "
          >
            <div className="mb-3 flex items-center gap-2">
              <Filter className="h-4 w-4 text-exchange-gold" />

              <span className="text-sm font-medium">
                Filters
              </span>

              {hasActiveFilters && (
                <button
                  type="button"
                  onClick={resetFilters}
                  className="
                    ml-auto
                    text-xs
                    text-exchange-muted
                    transition-colors
                    hover:text-exchange-text
                  "
                >
                  Reset
                </button>
              )}
            </div>

            <div className="grid gap-3 sm:grid-cols-2">
              <label className="block">
                <span className="mb-1.5 block text-xs text-exchange-muted">
                  Wallet
                </span>

                <select
                  value={selectedWallet}
                  onChange={(event) =>
                    setSelectedWallet(event.target.value)
                  }
                  className="
                    w-full
                    rounded-2xl
                    border
                    border-exchange-border
                    bg-black/20
                    px-3
                    py-2.5
                    text-sm
                    text-exchange-text
                    outline-none
                    transition-colors
                    focus:border-exchange-gold
                  "
                >
                  <option
                    value={ALL_WALLETS}
                    className="bg-exchange-card"
                  >
                    All wallets
                  </option>

                  {currencies.map((currency) => (
                    <option
                      key={currency}
                      value={currency}
                      className="bg-exchange-card"
                    >
                      {currency}
                    </option>
                  ))}
                </select>
              </label>

              <label className="block">
                <span className="mb-1.5 block text-xs text-exchange-muted">
                  Type
                </span>

                <select
                  value={selectedType}
                  onChange={(event) =>
                    setSelectedType(event.target.value)
                  }
                  className="
                    w-full
                    rounded-2xl
                    border
                    border-exchange-border
                    bg-black/20
                    px-3
                    py-2.5
                    text-sm
                    text-exchange-text
                    outline-none
                    transition-colors
                    focus:border-exchange-gold
                  "
                >
                  <option
                    value={ALL_TYPES}
                    className="bg-exchange-card"
                  >
                    All types
                  </option>

                  {operationTypes.map((type) => (
                    <option
                      key={type}
                      value={type}
                      className="bg-exchange-card"
                    >
                      {type.charAt(0).toUpperCase() +
                        type.slice(1)}
                    </option>
                  ))}
                </select>
              </label>
            </div>
          </section>
        )}

      {isLoading && (
        <div
          className="
            rounded-3xl
            border
            border-exchange-border
            bg-exchange-card
            p-5
            text-sm
            text-exchange-muted
          "
        >
          Loading history...
        </div>
      )}

      {!isLoading && isError && (
        <div
          className="
            rounded-3xl
            border
            border-red-500/20
            bg-exchange-card
            p-5
            text-sm
            text-red-400
          "
        >
          Failed to load transaction history.
        </div>
      )}

      {!isLoading &&
        !isError &&
        transactions.length === 0 && (
          <div
            className="
              rounded-3xl
              border
              border-exchange-border
              bg-exchange-card
              p-6
              text-center
            "
          >
            <p className="font-medium">
              No transaction history
            </p>

            <p className="mt-1 text-sm text-exchange-muted">
              Your completed operations will appear here.
            </p>
          </div>
        )}

      {!isLoading &&
        !isError &&
        transactions.length > 0 &&
        filteredTransactions.length === 0 && (
          <div
            className="
              rounded-3xl
              border
              border-exchange-border
              bg-exchange-card
              p-6
              text-center
            "
          >
            <p className="font-medium">
              No matching operations
            </p>

            <p className="mt-1 text-sm text-exchange-muted">
              Try changing or resetting the filters.
            </p>

            <button
              type="button"
              onClick={resetFilters}
              className="
                mt-4
                rounded-2xl
                border
                border-exchange-border
                px-4
                py-2
                text-sm
                font-medium
                transition-colors
                hover:border-exchange-gold
                hover:text-exchange-gold
              "
            >
              Reset filters
            </button>
          </div>
        )}

      {!isLoading &&
        !isError &&
        filteredTransactions.length > 0 && (
          <TransactionHistory
            transactions={filteredTransactions}
          />
        )}
    </div>
  );
}
