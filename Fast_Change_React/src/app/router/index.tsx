import {
  createBrowserRouter,
} from "react-router-dom";

import RegisterPage from "@/pages/auth/RegisterPage";
import DashboardPage from "@/pages/dashboard/DashboardPage";
import {
  ProtectedRoute,
} from "./ProtectedRoute";
import {
  PublicRoute,
} from "./PublicRoute";

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
    path: "/dashboard",
    element: (
      <ProtectedRoute>
        <DashboardPage />
      </ProtectedRoute>
    ),
  },
]);