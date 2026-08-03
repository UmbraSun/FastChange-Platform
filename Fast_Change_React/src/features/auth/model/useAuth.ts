import { useAuthStore } from "@/entities/auth/model/authStore";

export function useAuth() {
  const accessToken = useAuthStore(
    (state) => state.accessToken
  );
  return {
    isAuthenticated: Boolean(accessToken),
  };
}