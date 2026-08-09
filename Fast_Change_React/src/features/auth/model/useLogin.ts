import { useMutation } from "@tanstack/react-query";
import { loginUser, } from "../api/login";
import type { LoginRequest, } from "./dto";
import { useAuthStore, } from "@/entities/auth/model/authStore";

export function useLogin() {
  const setTokens =
    useAuthStore(
      state => state.setTokens
    );

  return useMutation({
    mutationFn: (
      data: LoginRequest
    ) => loginUser(data),

    onSuccess: (response) => {
      setTokens(
        response.accessToken,
        response.refreshToken
      );
    },
  });
}