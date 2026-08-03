import { z } from "zod";

export const registerSchema = z.object({
  email: z.email("Invalid email address"),
  password: z
    .string()
    .min(8, "Password must contain at least 8 characters"),
});


export type RegisterFormValues = z.infer<typeof registerSchema>;