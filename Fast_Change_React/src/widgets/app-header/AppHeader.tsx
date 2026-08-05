import { Bell } from "lucide-react";

export function AppHeader() {
  return (
    <header className="sticky top-0 z-50 border-b border-exchange-border bg-exchange-bg/90 backdrop-blur">
      <div className="mx-auto flex h-16 max-w-7xl items-center justify-between px-4">
        <div>
          <h1 className="text-lg font-semibold">
            FastChange
          </h1>
          <p className="text-xs text-exchange-muted">
            Exchange Platform
          </p>
        </div>

        <button className="rounded-xl p-2 transition hover:bg-white/5">
          <Bell className="h-5 w-5 text-exchange-gold" />
        </button>
      </div>
    </header>
  );
}