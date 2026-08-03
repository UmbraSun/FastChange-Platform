import { RegisterForm } from "@/features/auth/ui/RegisterForm";

export default function RegisterPage() {
  return (
    <main className="min-h-screen bg-exchange-bg flex items-center justify-center px-4">
      <div className="w-full max-w-md">
        <RegisterForm />
      </div>
    </main>
  );
}