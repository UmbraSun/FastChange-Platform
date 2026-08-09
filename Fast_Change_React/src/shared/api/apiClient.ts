import axios, { type AxiosError, type AxiosResponse, } from "axios";

import { authInterceptor } from "./interceptors/authInterceptor";
import { useAuthStore } from "@/entities/auth/model/authStore";

export const apiClient = axios.create({
  baseURL:
    import.meta.env.VITE_API_URL ||
    "https://localhost:7289/api",

  headers: {
    "Content-Type": "application/json",
  },
});

apiClient.interceptors.request.use(
  authInterceptor
);

apiClient.interceptors.response.use(
  (response: AxiosResponse) => response,

  (error: AxiosError) => {
    if (error.response?.status === 401) {
      useAuthStore
        .getState()
        .clearTokens();
    }

    if (error.response?.data) {
      const problem =
        error.response.data as {
          status?: number;
          title?: string;
          errors?: unknown;
        };

      console.error(
        `Backend Error [${problem.status}]: ${problem.title}`,
        problem.errors
      );
    }

    return Promise.reject(error);
  }
);