import { useMutation } from "@tanstack/react-query";

import { askAssistant } from "@/entities/chat/api/chatApi";

export function useChat() {
  return useMutation({
    mutationFn: askAssistant,
  });
}
