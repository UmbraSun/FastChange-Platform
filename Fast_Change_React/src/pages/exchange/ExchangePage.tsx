import { ExchangeCard, } from "@/features/exchange/ui/ExchangeCard";

export default function ExchangePage() {
  return (
    <div className="space-y-6">
      <section>
        <h1 className="
          text-2xl
          font-semibold
        ">
          Exchange
        </h1>
        <p className="
          mt-1
          text-sm
          text-exchange-muted
        ">
          Swap your assets instantly
        </p>
      </section>
      <ExchangeCard />
    </div>
  );
}