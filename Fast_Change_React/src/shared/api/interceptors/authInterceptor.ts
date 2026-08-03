import type { AxiosInstance } from "axios";
import { useAuthStore } from "@/entities/auth/model/authStore";

export function setupAuthInterceptor(
  client: AxiosInstance
) {
  client.interceptors.request.use(
    (config) => {
      const token =
        useAuthStore.getState()
          .accessToken;

      if (token) {
        config.headers.Authorization =
          `Bearer ${token}`;
      }

      return config;
    }
  );
}