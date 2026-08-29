import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import { AppProviders } from "@/app/providers/AppProviders";
import { Toaster } from "sonner";

createRoot(
  document.getElementById("root")!
)
.render(
  <StrictMode>
    <AppProviders />
    <Toaster />
  </StrictMode>
);
