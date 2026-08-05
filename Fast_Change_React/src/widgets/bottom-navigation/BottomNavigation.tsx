import {
  Home,
  Wallet,
  ArrowLeftRight,
  History,
  User,
} from "lucide-react";
import { NavLink } from "react-router-dom";

const items = [
  {
    to: "/dashboard",
    label: "Home",
    icon: Home,
  },
  {
    to: "/wallets",
    label: "Wallets",
    icon: Wallet,
  },
  {
    to: "/exchange",
    label: "Exchange",
    icon: ArrowLeftRight,
  },
  {
    to: "/history",
    label: "History",
    icon: History,
  },
  {
    to: "/profile",
    label: "Profile",
    icon: User,
  },
];

export function BottomNavigation() {
  return (
    <nav className="fixed bottom-0 left-0 right-0 border-t border-exchange-border bg-exchange-card">
      <div className="mx-auto grid max-w-7xl grid-cols-5">
        {items.map(({ to, label, icon: Icon }) => (
          <NavLink
            key={to}
            to={to}
            className={({ isActive }) =>
              `flex flex-col items-center gap-1 py-3 text-xs transition ${
                isActive
                  ? "text-exchange-gold"
                  : "text-exchange-muted"
              }`
            }
          >
            <Icon className="h-5 w-5" />
            <span>{label}</span>
          </NavLink>
        ))}
      </div>
    </nav>
  );
}