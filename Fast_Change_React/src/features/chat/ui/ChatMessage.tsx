import { Bot, UserRound } from "lucide-react";

interface ChatMessageProps {
  role: "user" | "assistant";
  content: string;
}

export function ChatMessage({
  role,
  content,
}: ChatMessageProps) {
  const isUser = role === "user";

  return (
    <div
      className={`flex gap-3 ${
        isUser ? "justify-end" : "justify-start"
      }`}
    >
      {!isUser && (
        <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-full border border-exchange-border bg-black/20">
          <Bot className="h-4 w-4 text-exchange-gold" />
        </div>
      )}

      <div
        className={`
          max-w-[85%]
          rounded-2xl
          px-4
          py-3
          text-sm
          leading-relaxed
          ${
            isUser
              ? "rounded-br-md bg-exchange-gold text-black"
              : "rounded-bl-md border border-exchange-border bg-exchange-card text-exchange-text"
          }
        `}
      >
        {content}
      </div>

      {isUser && (
        <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-full border border-exchange-border bg-exchange-card">
          <UserRound className="h-4 w-4 text-exchange-muted" />
        </div>
      )}
    </div>
  );
}
