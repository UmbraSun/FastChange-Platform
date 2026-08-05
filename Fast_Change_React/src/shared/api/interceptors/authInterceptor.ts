import type { InternalAxiosRequestConfig, } from "axios";
import { useAuthStore, } from "@/entities/auth/model/authStore";

export function authInterceptor(
  config: InternalAxiosRequestConfig
) {
  const token =
    useAuthStore.getState().accessToken;

  if (token) {
    config.headers.Authorization =
      `Bearer ${token}`;
  }

  return config;
}