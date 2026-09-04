import { MessageCircle } from "lucide-react";

import { ChatPanel } from "@/features/chat/ui/ChatPanel";

export default function AssistantPage() {
  return (
    <div className="mx-auto w-full max-w-4xl space-y-6">
      <header className="px-1">
        <div className="flex items-center gap-2">
          <MessageCircle className="h-5 w-5 text-exchange-gold" />

          <h1 className="text-2xl font-semibold tracking-tight">
            Assistant
          </h1>
        </div>

        <p className="mt-1 text-sm text-exchange-muted">
          Your FastChange AI assistant
        </p>
      </header>

      <ChatPanel />
    </div>
  );
}
