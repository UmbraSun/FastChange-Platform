import { useState } from "react";
import {
  ArrowDownToLine,
  ArrowLeftRight,
  ArrowUpFromLine,
} from "lucide-react";
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
        <h1 className="text-2xl font-semibold tracking-tight">
          Wallets
        </h1>

        <p className="mt-1 text-sm text-exchange-muted">
          Manage your wallets and balances
        </p>
      </section>

      {isLoading ? (
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
            <div className="h-4 w-24 animate-pulse rounded bg-white/5" />

            <div className="mt-3 h-9 w-16 animate-pulse rounded-xl bg-white/5" />
          </section>

          <section className="grid grid-cols-3 gap-3">
            {[1, 2, 3].map((item) => (
              <div
                key={item}
                className="
                  h-24
                  animate-pulse
                  rounded-2xl
                  border
                  border-exchange-border
                  bg-exchange-card
                "
              />
            ))}
          </section>

          <section className="space-y-3">
            <div className="h-6 w-28 animate-pulse rounded bg-white/5" />

            {[1, 2].map((item) => (
              <div
                key={item}
                className="
                  h-24
                  animate-pulse
                  rounded-3xl
                  border
                  border-exchange-border
                  bg-exchange-card
                "
              />
            ))}
          </section>
        </>
      ) : isError ? (
        <section
          className="
            rounded-3xl
            border
            border-exchange-border
            bg-exchange-card
            p-6
          "
        >
          <p className="text-sm font-medium">
            Unable to load wallets
          </p>

          <p className="mt-1 text-sm text-exchange-muted">
            Please try again later.
          </p>
        </section>
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
                min-h-24
                flex-col
                items-center
                justify-center
                gap-2
                rounded-2xl
                border
                border-exchange-border
                bg-exchange-card
                p-4
                text-sm
                transition
                hover:border-exchange-gold/60
                hover:bg-exchange-gold/5
                focus-visible:outline-none
                focus-visible:ring-2
                focus-visible:ring-exchange-gold/50
                active:scale-[0.98]
              "
            >
              <ArrowDownToLine className="h-5 w-5 text-exchange-gold" />

              <span>
                Deposit
              </span>
            </button>

            <button
              type="button"
              onClick={() => setIsWithdrawOpen(true)}
              className="
                flex
                min-h-24
                flex-col
                items-center
                justify-center
                gap-2
                rounded-2xl
                border
                border-exchange-border
                bg-exchange-card
                p-4
                text-sm
                transition
                hover:border-exchange-gold/60
                hover:bg-exchange-gold/5
                focus-visible:outline-none
                focus-visible:ring-2
                focus-visible:ring-exchange-gold/50
                active:scale-[0.98]
              "
            >
              <ArrowUpFromLine className="h-5 w-5 text-exchange-gold" />

              <span>
                Withdraw
              </span>
            </button>

            <button
              type="button"
              onClick={() => navigate("/exchange")}
              className="
                flex
                min-h-24
                flex-col
                items-center
                justify-center
                gap-2
                rounded-2xl
                border
                border-exchange-border
                bg-exchange-card
                p-4
                text-sm
                transition
                hover:border-exchange-gold/60
                hover:bg-exchange-gold/5
                focus-visible:outline-none
                focus-visible:ring-2
                focus-visible:ring-exchange-gold/50
                active:scale-[0.98]
              "
            >
              <ArrowLeftRight className="h-5 w-5 text-exchange-gold" />

              <span>
                Exchange
              </span>
            </button>
          </section>

          {wallets.length === 0 ? (
            <section
              className="
                rounded-3xl
                border
                border-exchange-border
                bg-exchange-card
                p-6
              "
            >
              <p className="font-medium">
                No wallets available
              </p>

              <p className="mt-1 text-sm text-exchange-muted">
                Your wallets will appear here once they are available.
              </p>
            </section>
          ) : (
            <section className="space-y-3">
              <h2 className="text-lg font-semibold">
                All wallets
              </h2>

              <div className="space-y-3">
                {wallets.map((wallet) => (
                  <WalletCard
                    key={wallet.walletId}
                    wallet={wallet}
                  />
                ))}
              </div>
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
