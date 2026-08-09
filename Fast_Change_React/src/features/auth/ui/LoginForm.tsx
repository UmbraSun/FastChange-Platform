import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useNavigate } from "react-router-dom";

import { Input } from "@/shared/ui/input";
import { Button } from "@/shared/ui/button";

import {
  loginSchema,
  type LoginFormValues,
} from "../model/schema";

import { useLogin } from "../model/useLogin";

export function LoginForm() {
  const navigate = useNavigate();
  const loginMutation = useLogin();

  const {
    register,
    handleSubmit,
    formState: {
      errors,
    },
  } = useForm<LoginFormValues>({
    resolver: zodResolver(loginSchema),
  });

  const onSubmit = (
    values: LoginFormValues
  ) => {
    loginMutation.mutate(
      values,
      {
        onSuccess: () => {
          navigate("/dashboard");
        },
      }
    );
  };

  return (
    <div className="mx-auto w-full max-w-md space-y-6">
      <div>
        <h1 className="text-3xl font-bold">
          Welcome back
        </h1>

        <p className="mt-2 text-exchange-muted">
          Sign in to your FastChange account
        </p>
      </div>

      <form
        onSubmit={handleSubmit(onSubmit)}
        className="space-y-4"
      >
        <div>
          <Input
            placeholder="Email"
            type="email"
            {...register("email")}
          />

          {errors.email && (
            <p className="mt-1 text-sm text-exchange-danger">
              {errors.email.message}
            </p>
          )}
        </div>

        <div>
          <Input
            placeholder="Password"
            type="password"
            {...register("password")}
          />

          {errors.password && (
            <p className="mt-1 text-sm text-exchange-danger">
              {errors.password.message}
            </p>
          )}
        </div>

        {loginMutation.isError && (
          <p className="text-sm text-exchange-danger">
            Invalid email or password
          </p>
        )}

        <Button
          type="submit"
          className="w-full"
          disabled={loginMutation.isPending}
        >
          {loginMutation.isPending
            ? "Signing in..."
            : "Sign in"}
        </Button>
      </form>

      <div className="text-center text-sm text-exchange-muted">
        Don't have an account?{" "}
        <button
          type="button"
          onClick={() => navigate("/register")}
          className="font-medium text-exchange-gold hover:underline"
        >
          Create account
        </button>
      </div>
    </div>
  );
}