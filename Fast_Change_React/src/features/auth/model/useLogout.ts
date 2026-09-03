import { useNavigate } from "react-router-dom";
import { useAuthStore } from "@/entities/auth/model/authStore";

export function useLogout() {
  const navigate = useNavigate();
  const clearTokens = useAuthStore(
    (state) => state.clearTokens,
  );

  return () => {
    clearTokens();

    navigate("/login", {
      replace: true,
    });
  };
}