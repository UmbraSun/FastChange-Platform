import { WalletCard, } from "@/entities/wallet/ui/WalletCard";
import { useWallets, } from "@/entities/wallet/model/useWallets";

export function WalletsPreview() {
  const {
    data,
    isLoading,
  } = useWallets();

  if (isLoading) {
    return (
      <div>
        Loading wallets...
      </div>
    );
  }

  return (
    <section
      className="
        space-y-4
      "
    >
      <h2
        className="
          text-lg
          font-semibold
        "
      >
        Your wallets
      </h2>
      <div className="space-y-3">
        {data?.map((wallet) => (
          <WalletCard
            key={wallet.walletId}
            wallet={wallet}
          />
        ))}
      </div>
    </section>
  );
}