import { useMemo, useState } from "react";
import { useWallets } from "@/entities/wallet/model/useWallets";
import { useExchangePreview } from "../model/useExchangePreview";
import { useExchange } from "../model/useExchange";
import { Select } from "@/shared/ui/select/Select";

export function ExchangeCard() {
  const {
    data: wallets,
    isLoading,
    isError,
  } = useWallets();

  const [fromWalletId, setFromWalletId] = useState("");
  const [toWalletId, setToWalletId] = useState("");
  const [amount, setAmount] = useState("");

  const sortedWallets = useMemo(() => {
    return [...(wallets ?? [])].sort(
      (a, b) => a.currency.localeCompare(b.currency),
    );
  }, [wallets]);

  const fromWallet = sortedWallets.find(
    (wallet) => wallet.walletId === fromWalletId,
  );

  const toWallet = sortedWallets.find(
    (wallet) => wallet.walletId === toWalletId,
  );

  const numericAmount = Number(amount);

  const isAmountValid =
    Number.isFinite(numericAmount) &&
    numericAmount > 0 &&
    numericAmount <= (fromWallet?.balance ?? 0);

  const canExchange =
    Boolean(fromWallet) &&
    Boolean(toWallet) &&
    fromWalletId !== toWalletId &&
    isAmountValid;

  const preview = useExchangePreview({
    fromWalletId,
    toWalletId,
    amount: numericAmount,
  });

  const exchangeMutation = useExchange();

  const handleExchange = () => {
    if (!canExchange || exchangeMutation.isPending) {
      return;
    }

    exchangeMutation.mutate({
      fromWalletId,
      toWalletId,
      amount: numericAmount,
    });
  };

  if (isLoading) {
    return (
      <section className="rounded-3xl border border-exchange-border bg-exchange-card p-6">
        <div className="space-y-4">
          <div className="h-5 w-24 animate-pulse rounded bg-white/5" />
          <div className="h-16 animate-pulse rounded-2xl bg-white/5" />
          <div className="mx-auto h-8 w-8 animate-pulse rounded-full bg-white/5" />
          <div className="h-16 animate-pulse rounded-2xl bg-white/5" />
          <div className="h-16 animate-pulse rounded-2xl bg-white/5" />
          <div className="h-12 animate-pulse rounded-2xl bg-white/5" />
        </div>
      </section>
    );
  }

  if (isError) {
    return (
      <section className="rounded-3xl border border-exchange-border bg-exchange-card p-6">
        <div className="py-8 text-center">
          <p className="font-medium">
            Failed to load wallets
          </p>
          <p className="mt-1 text-sm text-exchange-muted">
            Please try again later.
          </p>
        </div>
      </section>
    );
  }

  if (sortedWallets.length < 2) {
    return (
      <section className="rounded-3xl border border-exchange-border bg-exchange-card p-6">
        <div className="py-8 text-center">
          <p className="font-medium">
            Not enough wallets
          </p>
          <p className="mt-1 text-sm text-exchange-muted">
            You need at least two wallets to exchange assets.
          </p>
        </div>
      </section>
    );
  }

  return (
    <section
      className="
        space-y-5
        rounded-3xl
        border
        border-exchange-border
        bg-exchange-card
        p-6
      "
    >
      <div>
        <p className="text-sm text-exchange-muted">
          You pay
        </p>

        <div
          className="
            mt-2
            flex
            items-center
            justify-between
            gap-3
            rounded-2xl
            border
            border-exchange-border
            bg-black/20
            p-4
          "
        >
          <Select
            value={fromWalletId}
            onChange={setFromWalletId}
            placeholder="Select wallet"
            options={sortedWallets.map((wallet) => ({
              value: wallet.walletId,
              label: wallet.currency,
            }))}
          />

          <input
            type="number"
            min="0"
            step="any"
            placeholder="0"
            value={amount}
            onChange={(event) => setAmount(event.target.value)}
            className="
              w-32
              min-w-0
              bg-transparent
              text-right
              text-xl
              font-semibold
              outline-none
            "
          />
        </div>

        <div className="mt-2 flex justify-between text-xs">
          <span className="text-exchange-muted">
            Balance
          </span>

          <span className="text-exchange-muted">
            {fromWallet
              ? `${fromWallet.balance} ${fromWallet.currency}`
              : "—"}
          </span>
        </div>

        {fromWallet &&
          Number.isFinite(numericAmount) &&
          numericAmount > fromWallet.balance && (
            <p className="mt-2 text-xs text-red-400">
              Insufficient balance.
            </p>
          )}
      </div>

      <div className="text-center text-2xl text-exchange-gold">
        ⇅
      </div>

      <div>
        <p className="text-sm text-exchange-muted">
          You receive
        </p>

        <div
          className="
            mt-2
            flex
            items-center
            justify-between
            gap-3
            rounded-2xl
            border
            border-exchange-border
            bg-black/20
            p-4
          "
        >
          <Select
            value={toWalletId}
            onChange={setToWalletId}
            placeholder="Select wallet"
            options={sortedWallets
              .filter(
                (wallet) =>
                  wallet.walletId !== fromWalletId,
              )
              .map((wallet) => ({
                value: wallet.walletId,
                label: wallet.currency,
              }))}
          />

          <span className="text-right text-xl font-semibold">
            {preview.isFetching
              ? "..."
              : preview.data
                ? preview.data.receivedAmount
                : "—"}
          </span>
        </div>

        {fromWalletId &&
          toWalletId &&
          fromWalletId === toWalletId && (
            <p className="mt-2 text-xs text-red-400">
              Source and destination wallets must be different.
            </p>
          )}
      </div>

      <div className="rounded-xl bg-black/20 p-4 text-sm">
        <div className="flex items-center justify-between gap-4">
          <span className="text-exchange-muted">
            Rate
          </span>

          <span className="text-right">
            {preview.data && fromWallet && toWallet
              ? `1 ${fromWallet.currency} ≈ ${preview.data.exchangeRate} ${toWallet.currency}`
              : "—"}
          </span>
        </div>
      </div>

      {exchangeMutation.isError && (
        <div className="rounded-xl border border-red-400/20 bg-red-400/5 p-3 text-sm text-red-400">
          Exchange failed. Please try again.
        </div>
      )}

      <button
        type="button"
        disabled={
          !canExchange ||
          exchangeMutation.isPending
        }
        onClick={handleExchange}
        className="
          w-full
          rounded-2xl
          bg-exchange-gold
          py-3
          font-semibold
          text-black
          transition
          hover:opacity-90
          disabled:cursor-not-allowed
          disabled:opacity-50
        "
      >
        {exchangeMutation.isPending
          ? "Processing..."
          : "Confirm Exchange"}
      </button>
    </section>
  );
}