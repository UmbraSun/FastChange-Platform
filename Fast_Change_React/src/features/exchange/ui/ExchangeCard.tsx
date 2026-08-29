import { useWallets } from "@/entities/wallet/model/useWallets";
import { useMemo, useState } from "react";
import { useExchangePreview } from "../model/useExchangePreview";
import { useExchange } from "../model/useExchange";
import { Select } from "@/shared/ui/select/Select";

export function ExchangeCard() {
  const {
    data: wallets,
    isLoading,
  } = useWallets();

  const [fromWalletId, setFromWalletId] = useState("");
  const [toWalletId, setToWalletId] = useState("");
  const [amount, setAmount] = useState("");

  const sortedWallets = useMemo(() => {
    return [...(wallets ?? [])].sort(
      (a, b) => a.currency.localeCompare(b.currency)
    );
  }, [wallets]);

  const fromWallet = sortedWallets.find(
    x => x.walletId === fromWalletId
  );

  const toWallet = sortedWallets.find(
    x => x.walletId === toWalletId
  );

  const preview = useExchangePreview({
    fromWalletId,
    toWalletId,
    amount: Number(amount),
  });

  const exchangeMutation = useExchange();

  const handleExchange = () => {
    exchangeMutation.mutate(
      {
        fromWalletId,
        toWalletId,
        amount: Number(amount),
      },
    );
  };

  if (isLoading) {
    return (
      <div className="text-center py-10">
        Loading wallets...
      </div>
    );
  }

  return (
    <section
      className="
        rounded-3xl
        border
        border-exchange-border
        bg-exchange-card
        p-6
        space-y-5
      "
    >
      <div>
        <p className="
          text-sm
          text-exchange-muted
          ">
          You pay
        </p>
        <div
          className="
            mt-2
            flex
            items-center
            justify-between
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
            placeholder="0"
            value={amount}
            onChange={(e) => setAmount(e.target.value)}
            className="
              w-32
              bg-transparent
              text-right
              text-xl
              font-semibold
              outline-none
            "
          />
        </div>
      </div>

      <p className="
        mt-2
        text-xs
        text-exchange-muted
      ">
        Balance:
        {" "}
        {fromWallet?.balance ?? 0}
        {" "}
        {fromWallet?.currency}
      </p>

      <div className="
        text-center
        text-2xl
        text-exchange-gold
      ">
        ⇅
      </div>
      <div>
        <p className="
          text-sm
          text-exchange-muted
        ">
          You receive
        </p>
        <div
          className="
            mt-2
            flex
            items-center
            justify-between
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
            options={sortedWallets.map((wallet) => ({
              value: wallet.walletId,
              label: wallet.currency,
            }))}
          />

          <span className="text-xl font-semibold">
            {
              preview.isFetching
                ? "..."
                : preview.data?.receivedAmount ?? 0
            }
          </span>
        </div>
      </div>
      <div
        className="
          rounded-xl
          bg-black/20
          p-4
          text-sm
        "
      >
        <div className="flex justify-between">
          <span className="text-exchange-muted">
            Rate
          </span>

          <span>
            1 {fromWallet?.currency}
            {" ≈ "}
            {preview.data?.exchangeRate ?? 0}
            {" "}
            {toWallet?.currency}
          </span>
        </div>
      </div>
      <button
        disabled={exchangeMutation.isPending}
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
        {
          exchangeMutation.isPending
            ? "Processing..."
            : "Confirm Exchange"
        }
      </button>
    </section>
  );
}