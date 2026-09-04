import { apiClient } from "@/shared/api/apiClient";
import type {
  ChatRequest,
  ChatResponse,
} from "../model/dto";

export async function askAssistant(
  data: ChatRequest,
): Promise<ChatResponse> {
  const response = await apiClient.post<ChatResponse>(
    "/chat",
    data,
  );

  return response.data;
}
