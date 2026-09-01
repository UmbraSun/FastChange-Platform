import { createBrowserRouter, } from "react-router-dom";
import { ProtectedRoute, } from "./ProtectedRoute";
import { PublicRoute, } from "./PublicRoute";
import { AppShell, } from "@/widgets/app-shell";

import LoginPage from "@/pages/auth/LoginPage";
import RegisterPage from "@/pages/auth/RegisterPage";
import DashboardPage from "@/pages/dashboard/DashboardPage";
import ExchangePage from "@/pages/exchange/ExchangePage";
import TransferPage from "@/pages/transfer/TransferPage";
import WalletsPage from "@/pages/wallets/WalletsPage";

export const router = createBrowserRouter([
  {
    path: "/",
    element: (
      <PublicRoute>
        <LoginPage />
      </PublicRoute>
    ),
  },
  {
    path: "/login",
    element: (
      <PublicRoute>
        <LoginPage />
      </PublicRoute>
    ),
  },
  {
    path: "/register",
    element: (
      <PublicRoute>
        <RegisterPage />
      </PublicRoute>
    ),
  },
  {
    path: "/",
    element: (
      <ProtectedRoute>
        <AppShell />
      </ProtectedRoute>
    ),
    children: [
      {
        path: "dashboard",
        element: <DashboardPage />,
      },
      {
        path: "wallets",
        element: <WalletsPage />,
      },
      {
        path: "exchange",
        element: <ExchangePage />,
      },
      {
        path: "transfer",
        element: <TransferPage />,
      },
    ],
  },
]);