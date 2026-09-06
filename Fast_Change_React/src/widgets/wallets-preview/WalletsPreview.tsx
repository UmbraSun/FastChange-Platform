import { WalletCard } from "@/entities/wallet/ui/WalletCard";
import { useWallets } from "@/entities/wallet/model/useWallets";

export function WalletsPreview() {
  const {
    data: wallets = [],
    isLoading,
    isError,
  } = useWallets();

  return (
    <section className="space-y-4">
      <div className="flex items-center justify-between px-1">
        <h2 className="text-lg font-semibold">
          Your wallets
        </h2>
      </div>

      {isLoading ? (
        <div className="space-y-3">
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
        </div>
      ) : isError ? (
        <div className="rounded-3xl border border-exchange-border bg-exchange-card p-4 text-sm text-exchange-muted">
          Failed to load wallets.
        </div>
      ) : wallets.length === 0 ? (
        <div className="rounded-3xl border border-exchange-border bg-exchange-card p-5 text-sm text-exchange-muted">
          No wallets available.
        </div>
      ) : (
        <div className="space-y-3">
          {wallets.map((wallet) => (
            <WalletCard
              key={wallet.walletId}
              wallet={wallet}
            />
          ))}
        </div>
      )}
    </section>
  );
}
