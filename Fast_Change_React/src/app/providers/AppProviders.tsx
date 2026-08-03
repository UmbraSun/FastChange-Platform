import type { ReactNode } from "react";
import { QueryClientProvider } from "@tanstack/react-query";
import { queryClient } from "@/shared/api/queryClient";
import { RouterProvider } from "./RouterProvider";

interface Props {
  children?: ReactNode;
}

export function AppProviders({
  children,
}: Props) {
  return (
    <QueryClientProvider client={queryClient}>
      <RouterProvider />
      {children}
    </QueryClientProvider>
  );
}