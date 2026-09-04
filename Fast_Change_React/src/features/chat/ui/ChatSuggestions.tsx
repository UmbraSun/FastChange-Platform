interface ChatSuggestionsProps {
  onSelect: (question: string) => void;
}

const suggestions = [
  "How does FastChange work?",
  "What currencies can I exchange?",
  "How does the exchange rate work?",
  "How can I transfer funds?",
];

export function ChatSuggestions({
  onSelect,
}: ChatSuggestionsProps) {
  return (
    <div className="space-y-3">
      <p className="text-xs font-medium uppercase tracking-wider text-exchange-muted">
        Try asking
      </p>

      <div className="flex flex-wrap gap-2">
        {suggestions.map((suggestion) => (
          <button
            key={suggestion}
            type="button"
            onClick={() => onSelect(suggestion)}
            className="
              rounded-2xl
              border
              border-exchange-border
              bg-exchange-card
              px-3
              py-2
              text-left
              text-sm
              text-exchange-text
              transition-colors
              hover:border-exchange-gold
              hover:text-exchange-gold
            "
          >
            {suggestion}
          </button>
        ))}
      </div>
    </div>
  );
}
