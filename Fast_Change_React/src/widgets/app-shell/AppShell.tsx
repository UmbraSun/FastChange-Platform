import { Outlet } from "react-router-dom";
import { AppHeader } from "@/widgets/app-header";
import { BottomNavigation } from "@/widgets/bottom-navigation";

export function AppShell() {
  return (
    <div className="min-h-screen bg-exchange-bg text-exchange-text">
      <AppHeader />

      <main className="mx-auto max-w-7xl px-4 pb-24 pt-4">
        <Outlet />
      </main>

      <BottomNavigation />
    </div>
  );
}