import { useMutation } from "@tanstack/react-query";
import {
  registerUser,
} from "../api/register";
import type {
  RegisterRequest,
} from "./dto";

export function useRegister() {
  return useMutation({
    mutationFn: (
      data: RegisterRequest
    ) => registerUser(data),
  });
}