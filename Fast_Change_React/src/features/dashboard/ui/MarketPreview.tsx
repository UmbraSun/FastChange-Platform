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
        <div className="text-sm text-exchange-muted">
          Loading markets...
        </div>
      ) : isError ? (
        <div className="text-sm text-exchange-muted">
          Failed to load markets
        </div>
      ) : markets.length === 0 ? (
        <div className="text-sm text-exchange-muted">
          No market data
        </div>
      ) : (
        <div className="space-y-4">
          {markets.map((market) => {
            const change =
              market.priceChangePercentage24h;

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
                className="flex items-center justify-between"
              >
                <div>
                  <p className="font-medium">
                    {symbols[market.currency] ?? market.currency.toUpperCase()}
                    /USD
                  </p>

                  <p className="text-sm text-exchange-muted">
                    ${market.price.toLocaleString("en-US", {
                      minimumFractionDigits: 2,
                      maximumFractionDigits: 2,
                    })}
                  </p>
                </div>

                <span className={changeClass}>
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