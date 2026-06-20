import React, { useState } from 'react';

export const RegisterForm: React.FC = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    // Action trigger goes here (e.g., dispatching to api)
    console.log({ email, password });
  };

  return (
    <div className="min-h-screen bg-exchangeBg text-exchangeText flex flex-col justify-between p-4 font-sans">
      {/* Top Navigation Bar Component (Mobile Native look) */}
      <header className="flex items-center justify-between py-2 border-b border-gray-800">
        <span className="text-xl font-bold tracking-wider text-exchangeGreen">FastChange</span>
        <button className="text-sm text-exchangeMuted font-medium">Log In</button>
      </header>

      {/* Main Form container: centered on desktop, full-width on mobile */}
      <main className="w-full max-w-md mx-auto my-auto py-6">
        <h1 className="text-2xl font-bold mb-2">Create Account</h1>
        <p className="text-sm text-exchangeMuted mb-6">Register to start trading and exchanging currency.</p>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-xs font-semibold text-exchangeMuted uppercase mb-1">Email Address</label>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              className="w-full bg-exchangeCard border border-gray-800 focus:border-exchangeGreen rounded px-3 py-3 text-sm focus:outline-none transition-colors"
              placeholder="name@example.com"
              required
            />
          </div>

          <div>
            <label className="block text-xs font-semibold text-exchangeMuted uppercase mb-1">Password</label>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="w-full bg-exchangeCard border border-gray-800 focus:border-exchangeGreen rounded px-3 py-3 text-sm focus:outline-none transition-colors"
              placeholder="Minimum 8 characters"
              required
            />
          </div>

          <div className="pt-2">
            <button
              type="submit"
              className="w-full bg-exchangeGreen hover:opacity-90 text-exchangeBg font-bold py-3.5 rounded text-sm transition-opacity shadow-md"
            >
              Sign Up
            </button>
          </div>
        </form>
      </main>

      {/* Footer for terms */}
      <footer className="text-center text-xs text-exchangeMuted py-4">
        By creating an account, you agree to our Terms of Service.
      </footer>
    </div>
  );
};