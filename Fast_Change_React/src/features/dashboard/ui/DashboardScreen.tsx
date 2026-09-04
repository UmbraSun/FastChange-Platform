import { DashboardHeader } from "./DashboardHeader";
import { BalanceCard } from "./BalanceCard";
import { QuickActions } from "./QuickActions";
import { AssistantPreview } from "./AssistantPreview";
import { MarketPreview } from "./MarketPreview";
import { RecentTransactions } from "./RecentTransactions";
import { WalletsPreview } from "@/widgets/wallets-preview";

export function DashboardScreen() {
  return (
    <div className="space-y-6">
      <DashboardHeader />
      <BalanceCard />
      <QuickActions />
      <AssistantPreview />
      <WalletsPreview />
      <MarketPreview />
      <RecentTransactions />
    </div>
  );
}