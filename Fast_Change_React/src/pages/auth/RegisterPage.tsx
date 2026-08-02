export const RegisterPage = () => {
  return (
    <div className="min-h-screen bg-exchange-bg flex items-center justify-center">
      <div className="rounded-xl bg-exchange-card p-8 border border-exchange-border">
        <h1 className="text-3xl font-bold text-exchange-text">
          FastChange
        </h1>

        <p className="mt-2 text-exchange-muted">
          Exchange platform
        </p>

        <button className="mt-6 rounded-lg bg-exchange-gold px-5 py-3 text-black">
          Continue
        </button>
      </div>
    </div>
  );
};