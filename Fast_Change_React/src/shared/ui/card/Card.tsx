import type { HTMLAttributes } from "react";
import { cn } from "@/shared/lib/cn";

interface CardProps extends HTMLAttributes<HTMLDivElement> {}

export function Card({
  className,
  ...props
}: CardProps) {
  return (
    <div
      className={cn(
        "rounded-2xl",
        "border",
        "border-exchange-border",
        "bg-exchange-card",
        "text-exchange-text",
        className
      )}
      {...props}
    />
  );
}