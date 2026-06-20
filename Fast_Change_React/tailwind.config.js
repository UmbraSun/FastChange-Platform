/** @type {import('tailwindcss').Config} */
export default {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  theme: {
    extend: {
      colors: {
        exchangeBg: '#0B0E11',       // Deep corporate dark (Binance style)
        exchangeCard: '#1E2329',     // Slightly lighter for cards/inputs
        exchangeGreen: '#00C087',    // Success / Buy / Crypto green
        exchangeRed: '#F6465D',      // Danger / Sell / Crypto red
        exchangeText: '#EAECEF',     // Main readable text
        exchangeMuted: '#848E9C',    // Secondary labels
      },
    },
  },
  plugins: [],
}