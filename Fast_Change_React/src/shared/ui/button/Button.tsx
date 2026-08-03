import type { ButtonHTMLAttributes } from "react";
import { cva, type VariantProps } from "class-variance-authority";
import { cn } from "@/shared/lib/cn";

const buttonVariants = cva(
  [
    "inline-flex",
    "items-center",
    "justify-center",
    "rounded-xl",
    "font-medium",
    "transition-colors",
    "focus-visible:outline-none",
    "disabled:pointer-events-none",
    "disabled:opacity-50",
  ],
  {
    variants: {
      variant: {
        primary:
          "bg-exchange-gold text-black hover:bg-exchange-gold-hover",
        secondary:
          "bg-exchange-card border border-exchange-border text-exchange-text hover:bg-neutral-800",
        ghost:
          "text-exchange-text hover:bg-exchange-card",
      },
      size: {
        sm: "h-9 px-3 text-sm",
        md: "h-11 px-5 text-sm",
        lg: "h-13 px-6 text-base",
      },
    },
    defaultVariants: {
      variant: "primary",
      size: "md",
    },
  }
);

interface ButtonProps
  extends ButtonHTMLAttributes<HTMLButtonElement>,
    VariantProps<typeof buttonVariants> {}

export function Button({
  className,
  variant,
  size,
  ...props
}: ButtonProps) {
  return (
    <button
      className={cn(
        buttonVariants({
          variant,
          size,
        }),
        className
      )}
      {...props}
    />
  );
}