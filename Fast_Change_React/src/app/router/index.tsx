import { Routes, Route, Navigate } from 'react-router-dom';

import { RegisterPage } from '@/pages/auth/RegisterPage';
import { DashboardPage } from '@/pages/dashboard/DashboardPage';

export const AppRouter = () => {
  return (
    <Routes>
      <Route
        path="/register"
        element={<RegisterPage />}
      />

      <Route
        path="/dashboard"
        element={<DashboardPage />}
      />

      <Route
        path="*"
        element={<Navigate to="/register" replace />}
      />
    </Routes>
  );
};