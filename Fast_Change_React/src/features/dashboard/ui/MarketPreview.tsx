import { useMarketOverview } from "@/entities/market/model/useMarketOverview";

export function MarketPreview() {
  const symbols: Record<string, string> = {
    bitcoin: "BTC",
    ethereum: "ETH",
    solana: "SOL",
  };

  const {
    data: markets = [],
    isLoading,
    isError,
  } = useMarketOverview();

  return (
    <section className="rounded-3xl border border-exchange-border bg-exchange-card p-5">
      <h2 className="mb-4 text-lg font-semibold">
        Markets
      </h2>

      {isLoading ? (
        <div className="space-y-4">
          {[1, 2, 3].map((item) => (
            <div
              key={item}
              className="flex items-center justify-between"
            >
              <div className="space-y-2">
                <div className="h-4 w-20 animate-pulse rounded bg-white/5" />
                <div className="h-3 w-24 animate-pulse rounded bg-white/5" />
              </div>

              <div className="h-4 w-16 animate-pulse rounded bg-white/5" />
            </div>
          ))}
        </div>
      ) : isError ? (
        <div className="rounded-2xl bg-black/10 p-4 text-sm text-exchange-muted">
          Failed to load market data.
        </div>
      ) : markets.length === 0 ? (
        <div className="rounded-2xl bg-black/10 p-4 text-sm text-exchange-muted">
          No market data available.
        </div>
      ) : (
        <div className="space-y-4">
          {markets.map((market) => {
            const change =
              market.priceChangePercentage24h;

            const baseCurrency =
              symbols[market.currency] ??
              market.currency.toUpperCase();

            const quoteCurrency =
              market.quoteCurrency.toUpperCase();

            const changeClass =
              change === null
                ? "text-exchange-muted"
                : change > 0
                  ? "text-green-400"
                  : change < 0
                    ? "text-red-400"
                    : "text-exchange-muted";

            return (
              <div
                key={`${market.currency}/${market.quoteCurrency}`}
                className="flex items-center justify-between gap-4"
              >
                <div className="min-w-0">
                  <p className="font-medium">
                    {baseCurrency}/{quoteCurrency}
                  </p>

                  <p className="text-sm text-exchange-muted">
                    {market.price.toLocaleString("en-US", {
                      minimumFractionDigits: 2,
                      maximumFractionDigits: 2,
                    })}{" "}
                    {quoteCurrency}
                  </p>
                </div>

                <span
                  className={`shrink-0 ${changeClass}`}
                >
                  {change === null
                    ? "—"
                    : `${change > 0 ? "+" : ""}${change.toFixed(2)}%`}
                </span>
              </div>
            );
          })}
        </div>
      )}
    </section>
  );
}
