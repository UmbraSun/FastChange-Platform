import { useMutation } from "@tanstack/react-query";
import { registerUser } from "../api/register";
import type { RegisterRequest } from "./dto";
import { useAuthStore } from "@/entities/auth/model/authStore";

export function useRegister() {
  const setTokens = useAuthStore(
    (state) => state.setTokens
  );

  return useMutation({
    mutationFn: (
      data: RegisterRequest
    ) => registerUser(data),

    onSuccess: (response) => {
      setTokens(
        response.accessToken,
        response.refreshToken
      );
    },
  });
}