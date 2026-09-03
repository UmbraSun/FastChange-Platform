import { LogOut, UserRound } from "lucide-react";

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

      <section className="overflow-hidden rounded-3xl border border-exchange-border bg-exchange-card">
        <div className="flex items-center gap-4 p-5">
          <div
            className="
              flex
              h-12
              w-12
              shrink-0
              items-center
              justify-center
              rounded-full
              bg-black/20
            "
          >
            <UserRound className="h-6 w-6 text-exchange-gold" />
          </div>

          <div>
            <p className="font-medium">
              FastChange Account
            </p>

            <p className="mt-0.5 text-sm text-exchange-muted">
              Your account settings
            </p>
          </div>
        </div>

        <div className="border-t border-exchange-border p-4">
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
            <div>
              <p className="font-medium">
                Log out
              </p>

              <p className="mt-0.5 text-xs text-red-400/70">
                Sign out of your FastChange account
              </p>
            </div>

            <LogOut className="h-5 w-5 shrink-0" />
          </button>
        </div>
      </section>
    </div>
  );
}