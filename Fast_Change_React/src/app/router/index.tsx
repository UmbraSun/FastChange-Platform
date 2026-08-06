import { createBrowserRouter, } from "react-router-dom";
import RegisterPage from "@/pages/auth/RegisterPage";
import DashboardPage from "@/pages/dashboard/DashboardPage";
import { ProtectedRoute, } from "./ProtectedRoute";
import { PublicRoute, } from "./PublicRoute";
import { AppShell, } from "@/widgets/app-shell";
import ExchangePage from "@/pages/exchange/ExchangePage";

export const router = createBrowserRouter([
  {
    path: "/",
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
        path: "exchange",
        element: <ExchangePage />,
      },
    ],
  },
]);