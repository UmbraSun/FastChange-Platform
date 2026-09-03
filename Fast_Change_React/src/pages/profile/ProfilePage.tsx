import { LogOut } from "lucide-react";
import { useLogout } from "@/features/auth/model/useLogout";

export default function ProfilePage() {
  const logout = useLogout();

  return (
    <div className="mx-auto w-full max-w-3xl space-y-7">
      <header className="px-1">
        <h1 className="text-2xl font-semibold tracking-tight">
          Profile
        </h1>

        <p className="mt-1 text-sm text-exchange-muted">
          Manage your account
        </p>
      </header>

      <section className="rounded-3xl border border-exchange-border bg-exchange-card p-5">
        <button
          type="button"
          onClick={logout}
          className="
            flex
            w-full
            items-center
            justify-between
            rounded-2xl
            border
            border-red-500/20
            px-4
            py-3
            text-left
            text-red-400
            transition-colors
            hover:bg-red-500/5
          "
        >
          <span className="font-medium">
            Log out
          </span>

          <LogOut className="h-5 w-5" />
        </button>
      </section>
    </div>
  );
}
