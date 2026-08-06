import {
  ArrowDownToLine,
  ArrowLeftRight,
  ArrowUpFromLine,
} from "lucide-react";
import { useNavigate, } from "react-router-dom";

const actions = [
  {
    title: "Deposit",
    icon: ArrowDownToLine,
    path: "/deposit",
  },
  {
    title: "Exchange",
    icon: ArrowLeftRight,
    path: "/exchange",
  },
  {
    title: "Withdraw",
    icon: ArrowUpFromLine,
    path: "/withdraw",
  },
];

export function QuickActions() {
  const navigate = useNavigate();

  return (
    <section
      className="
        grid
        grid-cols-3
        gap-3
      "
    >
      {
        actions.map(
          ({
            title,
            icon: Icon,
            path,
          }) => (
            <button
              key={title}
              onClick={() => navigate(path)}
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
        )
      }
    </section>
  );
}