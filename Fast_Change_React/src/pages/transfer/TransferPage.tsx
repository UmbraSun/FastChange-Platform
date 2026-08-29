import { TransferCard } from "@/features/transfer/ui/TransferCard";

export default function TransferPage() {
  return (
    <div className="space-y-6">
      <section>
        <h1 className="text-2xl font-semibold">
          Transfer
        </h1>

        <p className="mt-1 text-sm text-exchange-muted">
          Send funds to another FastChange user
        </p>
      </section>

      <TransferCard />
    </div>
  );
}
