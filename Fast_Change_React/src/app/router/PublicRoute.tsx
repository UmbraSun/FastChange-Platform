import { Navigate } from "react-router-dom";
import { useAuth } from "@/features/auth/model/useAuth";

interface Props {
  children: React.ReactNode;
}

export function PublicRoute({
  children,
}: Props) {
  const {
    isAuthenticated,
  } = useAuth();

  if (isAuthenticated) {
    return (
      <Navigate
        to="/dashboard"
        replace
      />
    );
  }

  return children;
}