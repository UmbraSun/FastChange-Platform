import { useState } from "react";
import { ArrowDownToLine, ArrowLeftRight, ArrowUpFromLine } from "lucide-react";
import { useNavigate } from "react-router-dom";

import { useWallets } from "@/entities/wallet/model/useWallets";
import { WalletCard } from "@/entities/wallet/ui/WalletCard";
import { DepositModal } from "@/features/wallet/ui/DepositModal";
import { WithdrawModal } from "@/features/wallet/ui/WithdrawModal";

export default function WalletsPage() {
  const navigate = useNavigate();

  const {
    data: wallets = [],
    isLoading,
    isError,
  } = useWallets();

  const [isDepositOpen, setIsDepositOpen] = useState(false);
  const [isWithdrawOpen, setIsWithdrawOpen] = useState(false);

  const walletCount = wallets.length;

  return (
    <div className="space-y-6">
      <section>
        <h1 className="text-2xl font-semibold">
          Wallets
        </h1>

        <p className="mt-1 text-sm text-exchange-muted">
          Manage your wallets and balances
        </p>
      </section>

      {isLoading ? (
        <div
          className="
            rounded-3xl
            border
            border-exchange-border
            bg-exchange-card
            p-6
            text-sm
            text-exchange-muted
          "
        >
          Loading wallets...
        </div>
      ) : isError ? (
        <div
          className="
            rounded-3xl
            border
            border-exchange-border
            bg-exchange-card
            p-6
            text-sm
            text-red-400
          "
        >
          Failed to load wallets
        </div>
      ) : (
        <>
          <section
            className="
              rounded-3xl
              border
              border-exchange-border
              bg-exchange-card
              p-6
            "
          >
            <p className="text-sm text-exchange-muted">
              Your wallets
            </p>

            <div className="mt-2 flex items-end gap-2">
              <span className="text-3xl font-semibold">
                {walletCount}
              </span>

              <span className="pb-1 text-sm text-exchange-muted">
                {walletCount === 1 ? "wallet" : "wallets"}
              </span>
            </div>
          </section>

          <section className="grid grid-cols-3 gap-3">
            <button
              type="button"
              onClick={() => setIsDepositOpen(true)}
              className="
                flex
                flex-col
                items-center
                gap-2
                rounded-2xl
                border
                border-exchange-border
                bg-exchange-card
                p-4
                transition
                hover:border-exchange-gold
              "
            >
              <ArrowDownToLine className="h-5 w-5 text-exchange-gold" />

              <span className="text-sm">
                Deposit
              </span>
            </button>

            <button
              type="button"
              onClick={() => setIsWithdrawOpen(true)}
              className="
                flex
                flex-col
                items-center
                gap-2
                rounded-2xl
                border
                border-exchange-border
                bg-exchange-card
                p-4
                transition
                hover:border-exchange-gold
              "
            >
              <ArrowUpFromLine className="h-5 w-5 text-exchange-gold" />

              <span className="text-sm">
                Withdraw
              </span>
            </button>

            <button
              type="button"
              onClick={() => navigate("/exchange")}
              className="
                flex
                flex-col
                items-center
                gap-2
                rounded-2xl
                border
                border-exchange-border
                bg-exchange-card
                p-4
                transition
                hover:border-exchange-gold
              "
            >
              <ArrowLeftRight className="h-5 w-5 text-exchange-gold" />

              <span className="text-sm">
                Exchange
              </span>
            </button>
          </section>

          {wallets.length === 0 ? (
            <div
              className="
                rounded-3xl
                border
                border-exchange-border
                bg-exchange-card
                p-6
                text-sm
                text-exchange-muted
              "
            >
              No wallets available
            </div>
          ) : (
            <section className="space-y-3">
              <h2 className="text-lg font-semibold">
                All wallets
              </h2>

              {wallets.map((wallet) => (
                <WalletCard
                  key={wallet.walletId}
                  wallet={wallet}
                />
              ))}
            </section>
          )}
        </>
      )}

      <DepositModal
        open={isDepositOpen}
        onClose={() => setIsDepositOpen(false)}
      />

      <WithdrawModal
        open={isWithdrawOpen}
        onClose={() => setIsWithdrawOpen(false)}
      />
    </div>
  );
}
