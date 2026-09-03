import axios, {
  type AxiosError,
  type AxiosRequestConfig,
  type AxiosResponse,
} from "axios";

import { authInterceptor } from "./interceptors/authInterceptor";
import { useAuthStore } from "@/entities/auth/model/authStore";
import { refreshAccessToken } from "@/features/auth/api/refresh";

export const apiClient = axios.create({
  baseURL:
    import.meta.env.VITE_API_URL ||
    "https://localhost:7289/api",

  headers: {
    "Content-Type": "application/json",
  },
});

apiClient.interceptors.request.use(
  authInterceptor,
);

let refreshPromise: Promise<string> | null = null;

function isRefreshRequest(
  config?: AxiosRequestConfig,
): boolean {
  return Boolean(config?.skipAuthRefresh);
}

async function refreshToken(): Promise<string> {
  const {
    refreshToken: currentRefreshToken,
  } = useAuthStore.getState();

  if (!currentRefreshToken) {
    throw new Error(
      "Refresh token is missing.",
    );
  }

  if (!refreshPromise) {
    refreshPromise = refreshAccessToken(
      currentRefreshToken,
    )
      .then((response) => {
        useAuthStore
          .getState()
          .setTokens(
            response.accessToken,
            response.refreshToken,
          );

        return response.accessToken;
      })
      .finally(() => {
        refreshPromise = null;
      });
  }

  return refreshPromise;
}

apiClient.interceptors.response.use(
  (response: AxiosResponse) => response,

  async (error: AxiosError) => {
    const originalRequest =
      error.config;

    if (
      error.response?.status === 401 &&
      originalRequest &&
      !isRefreshRequest(originalRequest) &&
      !originalRequest._retry
    ) {
      originalRequest._retry = true;

      try {
        const accessToken =
          await refreshToken();

        originalRequest.headers.Authorization =
          `Bearer ${accessToken}`;

        return apiClient.request(
          originalRequest,
        );
      } catch {
        useAuthStore
          .getState()
          .clearTokens();

        return Promise.reject(error);
      }
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
        problem.errors,
      );
    }

    return Promise.reject(error);
  },
);