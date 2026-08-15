import type { ReactNode } from "react";
import { QueryClientProvider } from "@tanstack/react-query";
import { queryClient } from "@/shared/api/queryClient";
import { RouterProvider } from "./RouterProvider";
import { WalletRealtimeProvider } from "./WalletRealtimeProvider";

interface Props {
  children?: ReactNode;
}

export function AppProviders({
  children,
}: Props) {
  return (
    <QueryClientProvider client={queryClient}>
      <WalletRealtimeProvider />
      <RouterProvider />
      {children}
    </QueryClientProvider>
  );
}