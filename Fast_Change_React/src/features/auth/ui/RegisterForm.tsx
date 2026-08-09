import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useNavigate } from "react-router-dom";
import { Input } from "@/shared/ui/input";
import { Button } from "@/shared/ui/button";
import { registerSchema, type RegisterFormValues, } from "../model/schema";
import { useRegister } from "../model/useRegister";

export function RegisterForm() {
  const navigate = useNavigate();
  const registerMutation = useRegister();

  const {
    register,
    handleSubmit,
    formState: {
      errors,
    },
  } = useForm<RegisterFormValues>({
    resolver: zodResolver(registerSchema),
  });

  const onSubmit = (
    values: RegisterFormValues
  ) => {
    registerMutation.mutate(
      values,
      {
        onSuccess: () => {
          navigate("/login");
        },

        onError: (error) => {
          console.error(
            "Registration failed",
            error
          );
        },
      }
    );
  };

  return (
    <div className="mx-auto w-full max-w-md space-y-6">
      <div>
        <h1 className="text-3xl font-bold">
          Create account
        </h1>
        <p className="mt-2 text-exchange-muted">
          Join FastChange exchange
        </p>
      </div>

      <form
        onSubmit={handleSubmit(onSubmit)}
        className="space-y-4"
      >
        <Input
          placeholder="Email"
          type="email"
          {...register("email")}
        />
        {errors.email && (
          <p className="text-sm text-exchange-danger">
            {errors.email.message}
          </p>
        )}

        <Input
          placeholder="Password"
          type="password"
          {...register("password")}
        />
        {errors.password && (
          <p className="text-sm text-exchange-danger">
            {errors.password.message}
          </p>
        )}

        <Button
          type="submit"
          className="w-full"
          disabled={registerMutation.isPending}
        >
          {registerMutation.isPending
            ? "Creating..."
            : "Create account"}
        </Button>

        <div className="text-center text-sm text-exchange-muted">
          Already have an account?{" "}
          <button
            type="button"
            onClick={() => navigate("/login")}
            className="font-medium text-exchange-gold hover:underline"
          >
            Sign in
          </button>
        </div>
      </form>
    </div>
  );
}