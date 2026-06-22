import React, { useState } from 'react';
import { RegisterForm } from './features/auth/components/RegisterForm';
import DashboardScreen from './features/dashboard/components/DashboardScreen';

export const App: React.FC = () => {
  // Simple view switcher to orchestrate between Auth and Dashboard phases without routing overhead yet
  const [currentView, setCurrentView] = useState<'register' | 'dashboard'>('register');

  const handleRegistrationSuccess = () => {
    setCurrentView('dashboard');
  };

  return (
    <div className="bg-exchangeBg min-h-screen selection:bg-exchangeGreen/30">
      {currentView === 'register' ? (
        <RegisterForm onSuccess={handleRegistrationSuccess} />
      ) : (
        <DashboardScreen />
      )}
    </div>
  );
};

export default App;