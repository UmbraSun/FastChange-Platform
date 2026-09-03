import { apiClient } from "@/shared/api/apiClient";
import type { RefreshTokenRequest, RefreshTokenResponse, } from "../model/dto";

export async function refreshAccessToken(
  refreshToken: string,
): Promise<RefreshTokenResponse> {
  const data: RefreshTokenRequest = {
    refreshToken,
  };

  const response =
    await apiClient.post<RefreshTokenResponse>(
      "/Auth/refresh",
      data,
      {
        skipAuthRefresh: true,
      },
    );

  return response.data;
}