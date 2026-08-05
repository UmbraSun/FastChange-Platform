export function BalanceCard() {
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
        <span className="
          rounded-full
          bg-exchange-gold/10
          px-3
          py-1
          text-xs
          text-exchange-gold
        ">
          USD
        </span>
      </div>

      <div className="
        mt-4
        text-4xl
        font-bold
      ">
        $12,450.00
      </div>

      <div className="
        mt-4
        flex
        items-center
        gap-2
        text-sm
      ">
        <span className="text-green-400">
          +2.45%
        </span>

        <span className="text-exchange-muted">
          today
        </span>
      </div>
    </section>
  );
}