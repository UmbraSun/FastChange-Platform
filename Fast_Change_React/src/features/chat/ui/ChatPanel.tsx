import { Bot, Send, Sparkles, } from "lucide-react";
import { useEffect, useRef, useState, } from "react";

import { useChat } from "../model/useChat";
import { ChatMessage } from "./ChatMessage";
import { ChatSuggestions } from "./ChatSuggestions";

interface Message {
  id: string;
  role: "user" | "assistant";
  content: string;
}

const MAX_QUESTION_LENGTH = 2000;

function createMessage(
  role: Message["role"],
  content: string,
): Message {
  return {
    id: crypto.randomUUID(),
    role,
    content,
  };
}

export function ChatPanel() {
  const [question, setQuestion] = useState("");
  const [messages, setMessages] = useState<Message[]>( [], );

  const messagesEndRef = useRef<HTMLDivElement | null>(null);
  const chat = useChat();
  const isSending = chat.isPending;

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({
      behavior: "smooth",
    });
  }, [messages, isSending]);

  async function sendQuestion(
    value: string = question,
  ) {
    const trimmedQuestion = value.trim();

    if (
      !trimmedQuestion ||
      isSending ||
      trimmedQuestion.length >
        MAX_QUESTION_LENGTH
    ) {
      return;
    }

    setQuestion("");

    const userMessage = createMessage(
      "user",
      trimmedQuestion,
    );

    setMessages((current) => [
      ...current,
      userMessage,
    ]);

    try {
      const response =
        await chat.mutateAsync({
          question: trimmedQuestion,
        });

      setMessages((current) => [
        ...current,
        createMessage(
          "assistant",
          response.answer,
        ),
      ]);
    } catch {
      setMessages((current) => [
        ...current,
        createMessage(
          "assistant",
          "Sorry, I couldn't process your request. Please try again.",
        ),
      ]);
    }
  }

  function handleSubmit(
    event: React.FormEvent<HTMLFormElement>,
  ) {
    event.preventDefault();

    void sendQuestion();
  }

  function handleKeyDown(
    event: React.KeyboardEvent<HTMLTextAreaElement>,
  ) {
    if (
      event.key === "Enter" &&
      !event.shiftKey
    ) {
      event.preventDefault();
      void sendQuestion();
    }
  }

  const hasMessages = messages.length > 0;

  return (
    <div className="flex min-h-[calc(100vh-10rem)] flex-col overflow-hidden rounded-3xl border border-exchange-border bg-exchange-card">
      <div className="border-b border-exchange-border px-4 py-4 sm:px-5">
        <div className="flex items-center gap-3">
          <div className="flex h-10 w-10 items-center justify-center rounded-full bg-black/20">
            <Bot className="h-5 w-5 text-exchange-gold" />
          </div>

          <div>
            <div className="flex items-center gap-2">
              <h2 className="font-semibold">
                FastChange Assistant
              </h2>

              <Sparkles className="h-4 w-4 text-exchange-gold" />
            </div>

            <p className="mt-0.5 text-xs text-exchange-muted">
              Ask questions about FastChange
            </p>
          </div>
        </div>
      </div>

      <div className="flex-1 overflow-y-auto px-4 py-5 sm:px-6">
        {!hasMessages ? (
          <div className="flex min-h-[55vh] flex-col justify-center">
            <div className="mx-auto w-full max-w-xl">
              <div className="mb-8 text-center">
                <div className="mx-auto mb-4 flex h-14 w-14 items-center justify-center rounded-full border border-exchange-border bg-black/20">
                  <Sparkles className="h-6 w-6 text-exchange-gold" />
                </div>

                <h3 className="text-xl font-semibold">
                  How can I help?
                </h3>

                <p className="mx-auto mt-2 max-w-md text-sm leading-relaxed text-exchange-muted">
                  Ask me about exchanges, wallets,
                  transfers, or how FastChange works.
                </p>
              </div>

              <ChatSuggestions
                onSelect={(value) => {
                  void sendQuestion(value);
                }}
              />
            </div>
          </div>
        ) : (
          <div className="mx-auto w-full max-w-3xl space-y-4">
            {messages.map((message) => (
              <ChatMessage
                key={message.id}
                role={message.role}
                content={message.content}
              />
            ))}

            {isSending && (
              <div className="flex items-center gap-3">
                <div className="flex h-9 w-9 items-center justify-center rounded-full border border-exchange-border bg-black/20">
                  <Bot className="h-4 w-4 text-exchange-gold" />
                </div>

                <div className="rounded-2xl rounded-bl-md border border-exchange-border bg-black/10 px-4 py-3">
                  <div className="flex items-center gap-1.5">
                    <span className="h-1.5 w-1.5 animate-pulse rounded-full bg-exchange-gold" />
                    <span className="h-1.5 w-1.5 animate-pulse rounded-full bg-exchange-gold [animation-delay:150ms]" />
                    <span className="h-1.5 w-1.5 animate-pulse rounded-full bg-exchange-gold [animation-delay:300ms]" />
                  </div>
                </div>
              </div>
            )}

            <div ref={messagesEndRef} />
          </div>
        )}
      </div>

      <div className="border-t border-exchange-border p-3 sm:p-4">
        <form
          onSubmit={handleSubmit}
          className="mx-auto flex w-full max-w-3xl items-end gap-2"
        >
          <textarea
            value={question}
            onChange={(event) =>
              setQuestion(
                event.target.value.slice(
                  0,
                  MAX_QUESTION_LENGTH,
                ),
              )
            }
            onKeyDown={handleKeyDown}
            placeholder="Ask something..."
            rows={1}
            disabled={isSending}
            className="
              max-h-32
              min-h-11
              flex-1
              resize-none
              rounded-2xl
              border
              border-exchange-border
              bg-black/20
              px-4
              py-3
              text-sm
              text-exchange-text
              outline-none
              placeholder:text-exchange-muted
              focus:border-exchange-gold
              disabled:cursor-not-allowed
              disabled:opacity-60
            "
          />

          <button
            type="submit"
            disabled={
              isSending ||
              !question.trim()
            }
            className="
              flex
              h-11
              w-11
              shrink-0
              items-center
              justify-center
              rounded-2xl
              bg-exchange-gold
              text-black
              transition-opacity
              hover:opacity-90
              disabled:cursor-not-allowed
              disabled:opacity-40
            "
            aria-label="Send message"
          >
            <Send className="h-4 w-4" />
          </button>
        </form>

        <p className="mx-auto mt-2 max-w-3xl px-1 text-[11px] text-exchange-muted">
          Enter to send · Shift + Enter for a new line
        </p>
      </div>
    </div>
  );
}
