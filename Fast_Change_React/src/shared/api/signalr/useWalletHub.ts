import { useEffect } from "react";
import { walletHub } from "./walletHub";
import { useAuthStore } from "@/entities/auth/model/authStore";

export function useWalletHub(
    onWalletUpdated: (walletId: string) => void,
) {
    const accessToken = useAuthStore(
        (state) => state.accessToken,
    );

    useEffect(() => {
        if (!accessToken) {
            return;
        }

        let cancelled = false;

        const handleWalletUpdated = (data: {
            walletId: string;
        }) => {
            onWalletUpdated(data.walletId);
        };

        walletHub.on(
            "WalletUpdated",
            handleWalletUpdated,
        );

        const start = async () => {
            try {
                if (
                    walletHub.state === "Disconnected" &&
                    !cancelled
                ) {
                    await walletHub.start();
                }
            } catch (error) {
                console.error(
                    "[WalletHub] Connection failed",
                    error,
                );
            }
        };

        void start();

        return () => {
            cancelled = true;

            walletHub.off(
                "WalletUpdated",
                handleWalletUpdated,
            );
        };
    }, [accessToken, onWalletUpdated]);
}