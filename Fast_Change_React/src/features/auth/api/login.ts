import { apiClient } from "@/shared/api/apiClient";
import type {
  LoginRequest,
  LoginResponse,
} from "../model/dto";

export async function loginUser(
  data: LoginRequest
): Promise<LoginResponse> {
  const response = await apiClient.post<LoginResponse>(
    "/Auth/login",
    data
  );

  return response.data;
}