import { MessageCircle, Sparkles } from "lucide-react";
import { useNavigate } from "react-router-dom";

export function AssistantPreview() {
  const navigate = useNavigate();

  return (
    <button
      type="button"
      onClick={() => navigate("/assistant")}
      className="
        group
        w-full
        rounded-3xl
        border
        border-exchange-border
        bg-exchange-card
        p-5
        text-left
        transition-colors
        hover:border-exchange-gold/50
      "
    >
      <div className="flex items-center gap-4">
        <div
          className="
            flex
            h-11
            w-11
            shrink-0
            items-center
            justify-center
            rounded-2xl
            bg-black/20
            transition-colors
            group-hover:bg-exchange-gold/10
          "
        >
          <MessageCircle className="h-5 w-5 text-exchange-gold" />
        </div>

        <div className="min-w-0 flex-1">
          <div className="flex items-center gap-2">
            <h2 className="font-semibold">
              FastChange Assistant
            </h2>

            <Sparkles className="h-4 w-4 text-exchange-gold" />
          </div>

          <p className="mt-1 text-sm text-exchange-muted">
            Ask about exchanges, wallets and transfers
          </p>
        </div>

        <span className="shrink-0 text-sm font-medium text-exchange-gold">
          Ask
        </span>
      </div>
    </button>
  );
}
