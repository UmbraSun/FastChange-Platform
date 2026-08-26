import { usePortfolioPerformance } from "@/entities/portfolio/model/usePortfolioPerformance";

export function BalanceCard() {
  const {
    data,
    isLoading,
    isError,
  } = usePortfolioPerformance("USD");

  const formattedValue = data
    ? new Intl.NumberFormat("en-US", {
      style: "currency",
      currency: data.currency,
    }).format(data.currentValue)
    : null;

  const changePercent = data?.changePercent ?? 0;
  const changeClass =
    changePercent > 0
      ? "text-green-400"
      : changePercent < 0
        ? "text-red-400"
        : "text-exchange-muted";

  return (
    <section
      className="
        rounded-3xl
        border
        border-exchange-border
        bg-exchange-card
        p-6
      "
    >
      <div className="flex items-center justify-between">
        <span className="text-sm text-exchange-muted">
          Total balance
        </span>

        <span
          className="
            rounded-full
            bg-exchange-gold/10
            px-3
            py-1
            text-xs
            text-exchange-gold
          "
        >
          {data?.currency ?? "USD"}
        </span>
      </div>

      <div
        className="
          mt-4
          text-4xl
          font-bold
        "
      >
        {isLoading
          ? "..."
          : isError
            ? "—"
            : formattedValue}
      </div>

      <div
        className="
          mt-4
          flex
          items-center
          gap-2
          text-sm
        "
      >
        <span className={changeClass}>
          {changePercent > 0 ? "+" : ""}
          {changePercent.toFixed(2)}%
        </span>

        <span className="text-exchange-muted">
          today
        </span>
      </div>
    </section>
  );
}