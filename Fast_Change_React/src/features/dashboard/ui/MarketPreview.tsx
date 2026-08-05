const pairs = [
  {
    symbol: "BTC/USDT",
    price: "118,432.15",
    change: "+2.84%",
  },
  {
    symbol: "ETH/USDT",
    price: "4,218.52",
    change: "+1.14%",
  },
  {
    symbol: "SOL/USDT",
    price: "198.24",
    change: "-0.43%",
  },
];

export function MarketPreview() {
  return (
    <section className="rounded-3xl border border-exchange-border bg-exchange-card p-5">
      <h2 className="mb-4 text-lg font-semibold">
        Markets
      </h2>
      <div className="space-y-4">
        {pairs.map((pair) => (
          <div
            key={pair.symbol}
            className="flex items-center justify-between"
          >
            <div>
              <p className="font-medium">
                {pair.symbol}
              </p>
              <p className="text-sm text-exchange-muted">
                ${pair.price}
              </p>
            </div>
            <span
              className={
                pair.change.startsWith("+")
                  ? "text-green-400"
                  : "text-red-400"
              }
            >
              {pair.change}
            </span>
          </div>
        ))}
      </div>
    </section>
  );
}