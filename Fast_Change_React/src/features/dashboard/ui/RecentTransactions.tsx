const items = [
  "Exchange BTC → USDT",
  "Deposit USDT",
  "Transfer to wallet",
];

export function RecentTransactions() {
  return (
    <section className="rounded-3xl border border-exchange-border bg-exchange-card p-5">
      <h2 className="mb-4 text-lg font-semibold">
        Recent activity
      </h2>

      <div className="space-y-3">
        {items.map((item) => (
          <div
            key={item}
            className="rounded-xl bg-black/10 p-3"
          >
            {item}
          </div>
        ))}
      </div>
    </section>
  );
}