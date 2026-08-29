import { ArrowDownToLine, ArrowLeftRight, ArrowUpFromLine, Send, } from "lucide-react";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import { DepositModal } from "@/features/wallet/ui/DepositModal";
import { WithdrawModal } from "@/features/wallet/ui/WithdrawModal";

const actions = [
  {
    title: "Deposit",
    icon: ArrowDownToLine,
    action: "deposit",
  },
  {
    title: "Exchange",
    icon: ArrowLeftRight,
    action: "exchange",
  },
  {
    title: "Withdraw",
    icon: ArrowUpFromLine,
    action: "withdraw",
  },
  {
    title: "Transfer",
    icon: Send,
    action: "transfer",
  },
];

export function QuickActions() {
  const navigate = useNavigate();
  const [isDepositOpen, setIsDepositOpen] = useState(false);
  const [isWithdrawOpen, setIsWithdrawOpen] = useState(false);
  const handleAction = (action: string) => {
    switch (action) {
      case "deposit":
        setIsDepositOpen(true);
        break;

      case "exchange":
        navigate("/exchange");
        break;

      case "withdraw":
        setIsWithdrawOpen(true);
        break;

      case "transfer":
        navigate("/transfer");
        break;
    }
  };

  return (
    <>
      <section
        className="
          grid
          grid-cols-2
          gap-3
          sm:grid-cols-4
        "
      >
        {actions.map(
          ({
            title,
            icon: Icon,
            action,
          }) => (
            <button
              key={title}
              type="button"
              onClick={() =>
                handleAction(action)
              }
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

      <DepositModal
        open={isDepositOpen}
        onClose={() =>
          setIsDepositOpen(false)
        }
      />

      <WithdrawModal
        open={isWithdrawOpen}
        onClose={() =>
          setIsWithdrawOpen(false)
        }
      />
    </>
  );
}