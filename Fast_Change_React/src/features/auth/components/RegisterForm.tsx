import React, { useState } from 'react';

interface RegisterFormProps {
  onSuccess: () => void;
}

export const RegisterForm: React.FC<RegisterFormProps> = ({ onSuccess }) => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    console.log('Sending structured payload to backend:', { email, password });
    onSuccess();
  };

  return (
    <div className="mx-auto max-w-md p-8 rounded-3xl bg-exchangeCard shadow-[0_20px_60px_-20px_rgba(0,0,0,0.55)] mt-16">
      <h1 className="text-3xl font-semibold text-exchangeText mb-6">Create your account</h1>
      <form onSubmit={handleSubmit} className="space-y-5">
        <label className="block text-sm text-exchangeText/80">
          Email
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            placeholder="you@example.com"
            required
            className="mt-2 w-full rounded-2xl border border-neutral-700 bg-[#131A22] px-4 py-3 text-sm text-white outline-none transition focus:border-exchangeGreen"
          />
        </label>

        <label className="block text-sm text-exchangeText/80">
          Password
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            placeholder="••••••••"
            required
            className="mt-2 w-full rounded-2xl border border-neutral-700 bg-[#131A22] px-4 py-3 text-sm text-white outline-none transition focus:border-exchangeGreen"
          />
        </label>

        <button
          type="submit"
          className="w-full rounded-2xl bg-exchangeGreen px-5 py-3 text-sm font-semibold text-exchangeBg transition hover:bg-[#0dbc75]"
        >
          Sign Up
        </button>
      </form>
    </div>
  );
};
