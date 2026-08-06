export function ExchangeCard() {
  return (
    <section
      className="
        rounded-3xl
        border
        border-exchange-border
        bg-exchange-card
        p-6
        space-y-5
      "
    >
      <div>
        <p className="
          text-sm
          text-exchange-muted
          ">
          You pay
        </p>
        <div
          className="
            mt-2
            flex
            items-center
            justify-between
            rounded-2xl
            border
            border-exchange-border
            bg-black/20
            p-4
          "
        >
          <span>
            BTC
          </span>
          <span className="text-xl font-semibold">
            0
          </span>
        </div>
      </div>

      <div className="
        text-center
        text-2xl
        text-exchange-gold
      ">
        ⇅
      </div>
      <div>
        <p className="
          text-sm
          text-exchange-muted
        ">
          You receive
        </p>
        <div
          className="
            mt-2
            flex
            items-center
            justify-between
            rounded-2xl
            border
            border-exchange-border
            bg-black/20
            p-4
          "
        >
          <span>
            USD
          </span>
          <span className="text-xl font-semibold">
            0
          </span>
        </div>
      </div>
      <div
        className="
          rounded-xl
          bg-black/20
          p-4
          text-sm
        "
      >
        <div className="flex justify-between">
          <span className="text-exchange-muted">
            Rate
          </span>

          <span>
            1 BTC ≈ 118432 USD
          </span>
        </div>
      </div>
      <button
        className="
          w-full
          rounded-2xl
          bg-exchange-gold
          py-3
          font-semibold
          text-black
          transition
          hover:opacity-90
        "
      >
        Confirm Exchange
      </button>
    </section>
  );
}