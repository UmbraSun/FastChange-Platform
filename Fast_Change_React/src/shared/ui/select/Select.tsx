import {
  ChevronDown,
} from "lucide-react";
import {
  useEffect,
  useRef,
  useState,
} from "react";

export interface SelectOption {
  value: string;
  label: string;
}

const EMPTY_GUID = "00000000-0000-0000-0000-000000000000";

const isPlaceholderValue = (
  value: string | null | undefined,
) => {
  if (typeof value !== "string") {
    return true;
  }

  const trimmed = value.trim();

  return trimmed === "" || trimmed.toLowerCase() === EMPTY_GUID;
};

interface SelectProps {
  value: string;
  onChange: (value: string) => void;
  options: SelectOption[];
  placeholder?: string;
  disabled?: boolean;
}

export function Select({
  value,
  onChange,
  options,
  placeholder = "Select",
  disabled = false,
}: SelectProps) {
  const [isOpen, setIsOpen] = useState(false);
  const containerRef = useRef<HTMLDivElement>(null);

  const normalizedOptions = Array.from(
    new Map(
      options
        .filter(
          (option) =>
            !isPlaceholderValue(option.value),
        )
        .map((option) => [option.value, option]),
    ).values(),
  );

  const selectedOption = normalizedOptions.find(
    (option) => option.value === value,
  );

  useEffect(() => {
    const handlePointerDown = (
      event: MouseEvent,
    ) => {
      if (
        containerRef.current &&
        !containerRef.current.contains(
          event.target as Node,
        )
      ) {
        setIsOpen(false);
      }
    };

    document.addEventListener(
      "mousedown",
      handlePointerDown,
    );

    return () => {
      document.removeEventListener(
        "mousedown",
        handlePointerDown,
      );
    };
  }, []);

  useEffect(() => {
    if (disabled || normalizedOptions.length === 0) {
      setIsOpen(false);
    }
  }, [disabled, normalizedOptions.length]);

  useEffect(() => {
    setIsOpen(false);
  }, [value]);

  const handleSelect = (optionValue: string) => {
    if (isPlaceholderValue(optionValue)) {
      return;
    }

    onChange(optionValue);
    setIsOpen(false);
  };

  return (
    <div
      ref={containerRef}
      className="relative w-full"
    >
      <button
        type="button"
        disabled={disabled}
        aria-expanded={isOpen}
        aria-haspopup="listbox"
        onClick={() => {
          if (!disabled) {
            setIsOpen((open) => !open);
          }
        }}
        className="
          flex
          w-full
          items-center
          justify-between
          bg-transparent
          text-left
          text-lg
          font-semibold
          outline-none
          disabled:cursor-not-allowed
          disabled:opacity-50
        "
      >
        <span
          className={
            selectedOption
              ? "text-exchange-text"
              : "text-exchange-muted"
          }
        >
          {selectedOption?.label ?? placeholder}
        </span>

        <ChevronDown
          className={`
            h-5
            w-5
            text-exchange-muted
            transition-transform
            ${isOpen ? "rotate-180" : ""}
          `}
        />
      </button>

      {isOpen && (
        <div
          role="listbox"
          className="
            absolute
            left-0
            right-0
            top-full
            z-50
            mt-2
            overflow-hidden
            rounded-2xl
            border
            border-exchange-border
            bg-exchange-card
            shadow-2xl
          "
        >
          {normalizedOptions.length === 0 ? (
            <div className="p-4 text-sm text-exchange-muted">
              No options
            </div>
          ) : (
            normalizedOptions.map((option) => {
              const isSelected =
                option.value === value;

              return (
                <button
                  key={`${option.value}-${option.label}`}
                  type="button"
                  onClick={() =>
                    handleSelect(option.value)
                  }
                  className={`
                    block
                    w-full
                    px-4
                    py-3
                    text-left
                    text-base
                    transition
                    ${
                      isSelected
                        ? "bg-exchange-gold/10 text-exchange-gold"
                        : "text-exchange-text hover:bg-black/20"
                    }
                  `}
                >
                  {option.label}
                </button>
              );
            })
          )}
        </div>
      )}
    </div>
  );
}
