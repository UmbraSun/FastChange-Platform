import { useEffect, useState } from "react";
import { X } from "lucide-react";
import { useQueryClient } from "@tanstack/react-query";
import { useWallets } from "@/entities/wallet/model/useWallets";
import { useDeposit } from "../model/useDeposit";

interface DepositModalProps {
  open: boolean;
  onClose: () => void;
}

export function DepositModal({
  open,
  onClose,
}: DepositModalProps) {
  const queryClient = useQueryClient();
  const {
    data: wallets,
    isLoading,
  } = useWallets();
  const depositMutation = useDeposit();
  const [walletId, setWalletId] = useState("");
  const [amount, setAmount] = useState("");
  const selectedWallet = wallets?.find(
    wallet => wallet.walletId === walletId
  );

  useEffect(() => {
    if (!open) {
      setWalletId("");
      setAmount("");
      depositMutation.reset();
    }
  }, [open]);

  if (!open) {
    return null;
  }

  const numericAmount = Number(amount);
  const canDeposit =
    Boolean(walletId) &&
    numericAmount > 0 &&
    !depositMutation.isPending;
  const handleDeposit = () => {
    if (!canDeposit) {
      return;
    }
    depositMutation.mutate(
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
      }
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
      onMouseDown={onClose}
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
        onMouseDown={(event) => event.stopPropagation()}
      >
        <div className="mb-6 flex items-center justify-between">
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
            onClick={onClose}
            className="
              rounded-full
              p-2
              text-exchange-muted
              transition
              hover:bg-black/20
              hover:text-exchange-text
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

            <select
              value={walletId}
              onChange={(event) =>
                setWalletId(event.target.value)
              }
              disabled={isLoading}
              className="
                mt-2
                w-full
                rounded-2xl
                border
                border-exchange-border
                bg-black/20
                p-4
                outline-none
                transition
                focus:border-exchange-gold
              "
            >
              <option value="">
                Select wallet
              </option>

              {wallets?.map((wallet) => (
                <option
                  key={wallet.walletId}
                  value={wallet.walletId}
                >
                  {wallet.currency}
                </option>
              ))}
            </select>
          </div>

          <div>
            <div className="flex items-center justify-between">
              <label className="text-sm text-exchange-muted">
                Amount
              </label>

              {selectedWallet && (
                <span className="text-xs text-exchange-muted">
                  Balance: {selectedWallet.balance}{" "}
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
                type="number"
                min="0"
                step="any"
                value={amount}
                onChange={(event) =>
                  setAmount(event.target.value)
                }
                placeholder="0.00"
                className="
                  w-full
                  bg-transparent
                  py-4
                  outline-none
                "
              />

              {selectedWallet && (
                <span className="text-sm font-medium">
                  {selectedWallet.currency}
                </span>
              )}
            </div>
          </div>

          {depositMutation.isError && (
            <p className="text-sm text-exchange-danger">
              Failed to deposit funds. Please try again.
            </p>
          )}

          {depositMutation.isSuccess && (
            <div
              className="
                rounded-2xl
                border
                border-exchange-border
                bg-black/20
                p-4
              "
            >
              <p className="text-sm text-exchange-muted">
                Deposit successful
              </p>

              <p className="mt-1 text-lg font-semibold">
                New balance:{" "}
                {depositMutation.data.newBalance}
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
              disabled:cursor-not-allowed
              disabled:opacity-50
            "
          >
            {depositMutation.isPending
              ? "Processing..."
              : "Deposit"}
          </button>
        </div>
      </div>
    </div>
  );
}