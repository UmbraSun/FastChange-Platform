import type { InputHTMLAttributes } from "react";
import { cn } from "@/shared/lib/cn";

interface InputProps extends InputHTMLAttributes<HTMLInputElement> {}

export function Input({
  className,
  ...props
}: InputProps) {
  return (
    <input
      className={cn(
        "h-12",
        "w-full",
        "rounded-xl",
        "border",
        "border-exchange-border",
        "bg-[#11161D]",
        "px-4",
        "text-sm",
        "text-exchange-text",
        "placeholder:text-exchange-muted",
        "outline-none",
        "transition",
        "focus:border-exchange-gold",
        className
      )}
      {...props}
    />
  );
}