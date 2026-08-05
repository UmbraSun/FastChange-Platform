import {
  ArrowDownToLine,
  ArrowLeftRight,
  ArrowUpFromLine,
} from "lucide-react";

const actions = [
  {
    title: "Deposit",
    icon: ArrowDownToLine,
  },
  {
    title: "Exchange",
    icon: ArrowLeftRight,
  },
  {
    title: "Withdraw",
    icon: ArrowUpFromLine,
  },
];

export function QuickActions() {
  return (
    <section className="
      grid
      grid-cols-3
      gap-3
    ">
      {actions.map(
        ({
          title,
          icon: Icon,
        }) => (
          <button
            key={title}
            className="
              flex
              flex-col
              items-center
              gap-3
              rounded-2xl
              border
              border-exchange-border
              bg-exchange-card
              p-4
              transition
              hover:border-exchange-gold
            "
          >
            <Icon
              className="
                h-6
                w-6
                text-exchange-gold
              "
            />
            <span className="text-sm">
              {title}
            </span>
          </button>
        )
      )}
    </section>
  );
}