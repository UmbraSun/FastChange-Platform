import { useCallback, useEffect, useState, } from "react";
import { X } from "lucide-react";
import { useQueryClient } from "@tanstack/react-query";
import { useWallets } from "@/entities/wallet/model/useWallets";
import { useDeposit } from "../model/useDeposit";
import { Select } from "@/shared/ui/select/Select";

interface DepositModalProps {
  open: boolean;
  onClose: () => void;
}

function formatBalance(
  balance: number,
  currency: string,
) {
  const decimals =
    currency.toUpperCase() === "BTC"
      ? 8
      : 2;

  return balance.toFixed(decimals);
}

export function DepositModal({
  open,
  onClose,
}: DepositModalProps) {
  const queryClient = useQueryClient();

  const {
    data: wallets = [],
    isLoading,
    isError,
  } = useWallets();

  const {
    mutate,
    reset,
    isPending,
    isError: isDepositError,
    isSuccess,
    data,
  } = useDeposit();

  const [walletId, setWalletId] = useState("");
  const [amount, setAmount] = useState("");

  const selectedWallet = wallets.find(
    (wallet) => wallet.walletId === walletId,
  );

  const handleClose = useCallback(() => {
    setWalletId("");
    setAmount("");
    reset();
    onClose();
  }, [onClose, reset]);

  useEffect(() => {
    if (!open) {
      return;
    }

    const handleKeyDown = (
      event: KeyboardEvent,
    ) => {
      if (event.key === "Escape") {
        handleClose();
      }
    };

    document.addEventListener(
      "keydown",
      handleKeyDown,
    );

    return () => {
      document.removeEventListener(
        "keydown",
        handleKeyDown,
      );
    };
  }, [open, onClose]);

  if (!open) {
    return null;
  }

  const numericAmount = Number(amount);

  const canDeposit =
    Boolean(selectedWallet) &&
    Number.isFinite(numericAmount) &&
    numericAmount > 0 &&
    !isPending;

  const handleDeposit = () => {
    if (!canDeposit) {
      return;
    }

    mutate(
      {
        walletId,
        amount: numericAmount,
      },
      {
        onSuccess: async () => {
          await queryClient.invalidateQueries({
            queryKey: ["user-wallets"],
          });
        },
      },
    );
  };

  return (
    <div
      className="
        fixed
        inset-0
        z-50
        flex
        items-end
        justify-center
        bg-black/70
        p-0
        sm:items-center
        sm:p-4
      "
      onMouseDown={(event) => {
        if (
          event.target ===
          event.currentTarget
        ) {
          handleClose();
        }
      }}
    >
      <div
        className="
          w-full
          max-w-md
          rounded-t-3xl
          border
          border-exchange-border
          bg-exchange-card
          p-6
          sm:rounded-3xl
        "
        onMouseDown={(event) =>
          event.stopPropagation()
        }
      >
        <div className="mb-6 flex items-start justify-between gap-4">
          <div>
            <h2 className="text-xl font-semibold">
              Deposit
            </h2>

            <p className="mt-1 text-sm text-exchange-muted">
              Add funds to your wallet
            </p>
          </div>

          <button
            type="button"
            onClick={handleClose}
            aria-label="Close deposit dialog"
            className="
              shrink-0
              rounded-full
              p-2
              text-exchange-muted
              transition
              hover:bg-black/20
              hover:text-exchange-text
              focus-visible:outline-none
              focus-visible:ring-2
              focus-visible:ring-exchange-gold/50
            "
          >
            <X className="h-5 w-5" />
          </button>
        </div>

        <div className="space-y-5">
          <div>
            <label className="text-sm text-exchange-muted">
              Asset
            </label>

            <div className="mt-2">
              <Select
                value={walletId}
                onChange={(value) => {
                  setWalletId(value);
                  setAmount("");
                  reset();
                }}
                disabled={
                  isLoading ||
                  wallets.length === 0
                }
                placeholder={
                  isLoading
                    ? "Loading wallets..."
                    : "Select wallet"
                }
                options={wallets.map(
                  (wallet) => ({
                    value: wallet.walletId,
                    label: wallet.currency,
                  }),
                )}
              />
            </div>

            {isError && (
              <p className="mt-2 text-sm text-exchange-danger">
                Failed to load wallets.
              </p>
            )}

            {!isLoading &&
              !isError &&
              wallets.length === 0 && (
                <p className="mt-2 text-sm text-exchange-muted">
                  No wallets available.
                </p>
              )}
          </div>

          <div>
            <div className="flex items-center justify-between gap-3">
              <label
                htmlFor="deposit-amount"
                className="text-sm text-exchange-muted"
              >
                Amount
              </label>

              {selectedWallet && (
                <span className="text-xs text-exchange-muted">
                  Balance:{" "}
                  {formatBalance(
                    selectedWallet.balance,
                    selectedWallet.currency,
                  )}{" "}
                  {selectedWallet.currency}
                </span>
              )}
            </div>

            <div
              className="
                mt-2
                flex
                items-center
                rounded-2xl
                border
                border-exchange-border
                bg-black/20
                px-4
                transition
                focus-within:border-exchange-gold
              "
            >
              <input
                id="deposit-amount"
                type="number"
                min="0"
                step="any"
                value={amount}
                onChange={(event) => {
                  setAmount(
                    event.target.value,
                  );
                  reset();
                }}
                placeholder="0.00"
                inputMode="decimal"
                className="
                  w-full
                  bg-transparent
                  py-4
                  outline-none
                  placeholder:text-exchange-muted/50
                "
              />

              {selectedWallet && (
                <span className="shrink-0 text-sm font-medium">
                  {selectedWallet.currency}
                </span>
              )}
            </div>
          </div>

          {isDepositError && (
            <div
              className="
                rounded-2xl
                border
                border-red-500/20
                bg-red-500/5
                p-4
                text-sm
                text-exchange-danger
              "
            >
              Failed to deposit funds.
              Please try again.
            </div>
          )}

          {isSuccess && data && (
            <div
              className="
                rounded-2xl
                border
                border-exchange-gold/20
                bg-exchange-gold/5
                p-4
              "
            >
              <p className="text-sm text-exchange-muted">
                Deposit successful
              </p>

              <p className="mt-1 text-lg font-semibold">
                New balance:{" "}
                {data.newBalance}{" "}
                {selectedWallet?.currency}
              </p>
            </div>
          )}

          <button
            type="button"
            onClick={handleDeposit}
            disabled={!canDeposit}
            className="
              w-full
              rounded-2xl
              bg-exchange-gold
              py-3
              font-semibold
              text-black
              transition
              hover:opacity-90
              focus-visible:outline-none
              focus-visible:ring-2
              focus-visible:ring-exchange-gold/50
              disabled:cursor-not-allowed
              disabled:opacity-50
            "
          >
            {isPending
              ? "Processing..."
              : "Deposit"}
          </button>
        </div>
      </div>
    </div>
  );
}
