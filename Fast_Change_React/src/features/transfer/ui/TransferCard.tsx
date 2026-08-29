import { useMemo, useState } from "react";
import { useWallets } from "@/entities/wallet/model/useWallets";
import { useTransfer } from "@/entities/transfer/model/useTransfer";
import { useTransferRecipients } from "@/entities/transfer/model/useTransferRecipients";
import { Select } from "@/shared/ui/select/Select";

export function TransferCard() {
  const {
    data: wallets,
    isLoading,
  } = useWallets();

  const [fromWalletId, setFromWalletId] = useState("");
  const [recipientQuery, setRecipientQuery] = useState("");
  const [selectedRecipientWalletId, setSelectedRecipientWalletId] =
    useState("");
  const [amount, setAmount] = useState("");

  const transferMutation = useTransfer();
  const resetForm = () => {
    setRecipientQuery("");
    setSelectedRecipientWalletId("");
    setAmount("");
  };

  const sortedWallets = useMemo(() => {
    return [...(wallets ?? [])].sort(
      (a, b) => a.currency.localeCompare(b.currency),
    );
  }, [wallets]);

  const fromWallet = sortedWallets.find(
    (wallet) => wallet.walletId === fromWalletId,
  );

  const recipients = useTransferRecipients(
    recipientQuery,
    fromWallet?.currency ?? "",
  );

  const selectedRecipient = recipients.data?.find(
    (recipient) =>
      recipient.walletId === selectedRecipientWalletId,
  );

  const numericAmount = Number(amount);

  const canTransfer =
    Boolean(fromWalletId) &&
    Boolean(selectedRecipientWalletId) &&
    numericAmount > 0 &&
    numericAmount <= (fromWallet?.balance ?? 0) &&
    !transferMutation.isPending;

  const handleTransfer = () => {
    if (!canTransfer) {
      return;
    }

    transferMutation.mutate(
      {
        fromWalletId,
        toWalletId: selectedRecipientWalletId,
        amount: numericAmount,
      },
      {
        onSuccess: () => {
          resetForm();
        },
      },
    );
  };

  if (isLoading) {
    return (
      <div className="py-10 text-center">
        Loading wallets...
      </div>
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
      {/* From wallet */}
      <div>
        <p className="text-sm text-exchange-muted">
          From wallet
        </p>

        <div
          className="
            mt-2
            rounded-2xl
            border
            border-exchange-border
            bg-black/20
            p-4
          "
        >
          <Select
            value={fromWalletId}
            onChange={(value) => {
              setFromWalletId(value);
              setRecipientQuery("");
              setSelectedRecipientWalletId("");
            }}
            placeholder="Select wallet"
            options={sortedWallets.map((wallet) => ({
              value: wallet.walletId,
              label: wallet.currency,
            }))}
          />
        </div>

        <p className="mt-2 text-xs text-exchange-muted">
          Balance:{" "}
          {fromWallet?.balance ?? 0}{" "}
          {fromWallet?.currency}
        </p>
      </div>

      {/* Recipient */}
      <div>
        <p className="text-sm text-exchange-muted">
          Recipient
        </p>

        <input
          type="email"
          placeholder="Search by email"
          value={recipientQuery}
          disabled={!fromWallet}
          onChange={(event) => {
            setRecipientQuery(event.target.value);
            setSelectedRecipientWalletId("");
          }}
          className="
            mt-2
            w-full
            rounded-2xl
            border
            border-exchange-border
            bg-black/20
            p-4
            outline-none
            placeholder:text-exchange-muted
            focus:border-exchange-gold
            disabled:cursor-not-allowed
            disabled:opacity-50
          "
        />

        {recipientQuery.trim() &&
          !selectedRecipientWalletId && (
            <div
              className="
                mt-2
                overflow-hidden
                rounded-2xl
                border
                border-exchange-border
                bg-exchange-card
              "
            >
              {recipients.isFetching ? (
                <div className="p-4 text-sm text-exchange-muted">
                  Searching...
                </div>
              ) : recipients.data?.length ? (
                recipients.data.map((recipient) => (
                  <button
                    key={recipient.walletId}
                    type="button"
                    onClick={() => {
                      setSelectedRecipientWalletId(
                        recipient.walletId,
                      );
                      setRecipientQuery("");
                    }}
                    className="
                      block
                      w-full
                      border-b
                      border-exchange-border
                      p-4
                      text-left
                      last:border-b-0
                      hover:bg-black/20
                    "
                  >
                    <p className="font-medium">
                      {recipient.email}
                    </p>

                    <p className="mt-1 text-xs text-exchange-muted">
                      {recipient.currency} wallet
                    </p>
                  </button>
                ))
              ) : (
                <div className="p-4 text-sm text-exchange-muted">
                  No recipients found
                </div>
              )}
            </div>
          )}

        {selectedRecipient && (
          <div
            className="
              mt-2
              rounded-2xl
              border
              border-exchange-gold/30
              bg-exchange-gold/5
              p-4
            "
          >
            <p className="text-sm font-medium">
              {selectedRecipient.email}
            </p>

            <p className="mt-1 text-xs text-exchange-muted">
              {selectedRecipient.currency} wallet
            </p>
          </div>
        )}
      </div>

      {/* Amount */}
      <div>
        <p className="text-sm text-exchange-muted">
          Amount
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
          <input
            type="number"
            min="0"
            step="any"
            placeholder="0"
            value={amount}
            onChange={(event) =>
              setAmount(event.target.value)
            }
            className="
              w-full
              bg-transparent
              text-xl
              font-semibold
              outline-none
              placeholder:text-exchange-muted
            "
          />

          <span className="ml-3 font-semibold">
            {fromWallet?.currency ?? ""}
          </span>
        </div>

        {fromWallet &&
          numericAmount > fromWallet.balance && (
            <p className="mt-2 text-xs text-red-400">
              Insufficient balance
            </p>
          )}
      </div>

      {/* Summary */}
      {fromWallet && selectedRecipient && numericAmount > 0 && (
        <div
          className="
            rounded-2xl
            bg-black/20
            p-4
            text-sm
          "
        >
          <div className="flex justify-between">
            <span className="text-exchange-muted">
              You send
            </span>

            <span>
              {numericAmount} {fromWallet.currency}
            </span>
          </div>

          <div className="mt-2 flex justify-between">
            <span className="text-exchange-muted">
              Recipient receives
            </span>

            <span>
              {numericAmount} {selectedRecipient.currency}
            </span>
          </div>
        </div>
      )}

      {/* Submit */}
      <button
        type="button"
        disabled={!canTransfer}
        onClick={handleTransfer}
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
        {transferMutation.isPending
          ? "Processing..."
          : "Confirm Transfer"}
      </button>
    </section>
  );
}
