import type { WalletDto } from "../model/dto";

interface Props {
  wallet: WalletDto;
}

const currencyInfo: Record<
  string,
  {
    name: string;
    color: string;
    icon: string;
    decimals: number;
  }
> = {
  BTC: {
    name: "Bitcoin",
    color: "bg-amber-500",
    icon: "₿",
    decimals: 8,
  },
  USD: {
    name: "US Dollar",
    color: "bg-emerald-500",
    icon: "$",
    decimals: 2,
  },
  EUR: {
    name: "Euro",
    color: "bg-blue-500",
    icon: "€",
    decimals: 2,
  },
};

export function WalletCard({ wallet }: Props) {
  const currency = wallet.currency.toUpperCase();

  const info = currencyInfo[currency] ?? {
    name: currency,
    color: "bg-neutral-500",
    icon: currency[0] ?? "?",
    decimals: 2,
  };

  return (
    <div
      className="
        rounded-2xl
        border
        border-exchange-border
        bg-exchange-card
        p-4
        transition-all
        duration-200
        hover:border-exchange-gold/50
      "
    >
      <div className="flex items-center justify-between gap-4">
        <div className="flex min-w-0 items-center gap-3">
          <div
            className={`
              ${info.color}
              flex
              h-12
              w-12
              shrink-0
              items-center
              justify-center
              rounded-full
              text-lg
              font-bold
              text-black
            `}
          >
            {info.icon}
          </div>

          <div className="min-w-0">
            <h3 className="font-semibold text-exchange-text">
              {currency}
            </h3>

            <p className="truncate text-sm text-exchange-muted">
              {info.name}
            </p>
          </div>
        </div>

        <div className="shrink-0 text-right">
          <p
            className="
              text-lg
              font-semibold
              tabular-nums
              text-exchange-text
            "
          >
            {wallet.balance.toFixed(info.decimals)}
          </p>

          <p className="text-xs text-exchange-muted">
            Balance
          </p>
        </div>
      </div>
    </div>
  );
}
