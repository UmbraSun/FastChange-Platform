import { HubConnectionBuilder, LogLevel, } from "@microsoft/signalr";
import { useAuthStore } from "@/entities/auth/model/authStore";

const HUB_URL = import.meta.env.VITE_API_URL
    ? `${import.meta.env.VITE_API_URL.replace("/api", "")}/hubs/wallet`
    : "https://localhost:7289/hubs/wallet";

export const walletHub = new HubConnectionBuilder()
    .withUrl(HUB_URL, {
        accessTokenFactory: () =>
            useAuthStore.getState().accessToken ?? "",
    })
    .withAutomaticReconnect()
    .configureLogging(LogLevel.Warning)
    .build();
