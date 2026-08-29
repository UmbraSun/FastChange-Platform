import { useEffect, useState } from "react";
import { X } from "lucide-react";
import { useQueryClient } from "@tanstack/react-query";
import { useWallets } from "@/entities/wallet/model/useWallets";
import { useWithdraw } from "../model/useWithdraw";
import { Select } from "@/shared/ui/select/Select";

interface WithdrawModalProps {
    open: boolean;
    onClose: () => void;
}

export function WithdrawModal({
    open,
    onClose,
}: WithdrawModalProps) {
    const queryClient = useQueryClient();

    const {
        data: wallets,
        isLoading,
    } = useWallets();

    const withdrawMutation = useWithdraw();

    const [walletId, setWalletId] = useState("");
    const [amount, setAmount] = useState("");

    const selectedWallet = wallets?.find(
        wallet => wallet.walletId === walletId
    );

    useEffect(() => {
        if (!open) {
            setWalletId("");
            setAmount("");
            withdrawMutation.reset();
        }
    }, [open]);

    if (!open) {
        return null;
    }

    const numericAmount = Number(amount);

    const hasInsufficientBalance =
        selectedWallet !== undefined &&
        Number.isFinite(numericAmount) &&
        numericAmount > 0 &&
        numericAmount > selectedWallet.balance;

    const canWithdraw =
        selectedWallet !== undefined &&
        Number.isFinite(numericAmount) &&
        numericAmount > 0 &&
        !hasInsufficientBalance &&
        !withdrawMutation.isPending;

    const handleWithdraw = () => {
        if (!canWithdraw) {
            return;
        }

        withdrawMutation.mutate(
            {
                walletId,
                amount: numericAmount,
            },
            {
                onSuccess: async () => {
                    await queryClient.invalidateQueries({
                        queryKey: ["user-wallets"],
                    });
                },
            }
        );
    };

    return (
        <div
            className="
                fixed
                inset-0
                z-50
                flex
                items-end
                justify-center
                bg-black/60
                sm:items-center
            "
            onMouseDown={(event) => {
                if (event.target === event.currentTarget) {
                    onClose();
                }
            }}
        >
            <div
                className="
                    w-full
                    max-w-md
                    rounded-t-3xl
                    border
                    border-exchange-border
                    bg-exchange-card
                    p-6     
                    sm:rounded-3xl
                "
                onMouseDown={(event) =>
                    event.stopPropagation()
                }
            >
                <div className="flex items-start justify-between">
                    <div>
                        <h2 className="text-xl font-semibold">
                            Withdraw
                        </h2>

                        <p className="mt-1 text-sm text-exchange-muted">
                            Remove funds from your wallet
                        </p>
                    </div>

                    <button
                        type="button"
                        onClick={onClose}
                        className="
                            rounded-full
                            p-2
                            text-exchange-muted
                            transition
                            hover:bg-black/20
                            hover:text-exchange-text
                        "
                    >
                        <X className="h-5 w-5" />
                    </button>
                </div>

                <div className="mt-6 space-y-5">
                    <div>
                        <label className="text-sm text-exchange-muted">
                            Asset
                        </label>

                        <Select
                            value={walletId}
                            onChange={setWalletId}
                            disabled={isLoading}
                            placeholder="Select wallet"
                            options={(wallets ?? []).map((wallet) => ({
                                value: wallet.walletId,
                                label: wallet.currency,
                            }))}
                        />
                    </div>

                    <div>
                        <div className="flex items-center justify-between">
                            <label className="text-sm text-exchange-muted">
                                Amount
                            </label>

                            {selectedWallet && (
                                <span className="text-xs text-exchange-muted">
                                    Balance: {selectedWallet.balance}{" "}
                                    {selectedWallet.currency}
                                </span>
                            )}
                        </div>

                        <div
                            className="
                                mt-2
                                flex
                                items-center
                                rounded-2xl
                                border
                                border-exchange-border
                                bg-black/20
                                px-4
                                transition
                                focus-within:border-exchange-gold
                            "
                        >
                            <input
                                type="number"
                                min="0"
                                step="any"
                                value={amount}
                                onChange={(event) =>
                                    setAmount(event.target.value)
                                }
                                placeholder="0.00"
                                className="
                                    w-full
                                    bg-transparent
                                    py-4
                                    outline-none
                                "
                            />

                            {selectedWallet && (
                                <span className="text-sm font-medium">
                                    {selectedWallet.currency}
                                </span>
                            )}
                        </div>
                    </div>

                    {hasInsufficientBalance && (
                        <p className="text-sm text-exchange-danger">
                            Insufficient balance.
                        </p>
                    )}

                    {withdrawMutation.isError && (
                        <p className="text-sm text-exchange-danger">
                            Failed to withdraw funds. Please try again.
                        </p>
                    )}

                    {withdrawMutation.isSuccess && (
                        <div
                            className="
                                rounded-2xl
                                border
                                border-exchange-border
                                bg-black/20
                                p-4
                            "
                        >
                            <p className="text-sm text-exchange-muted">
                                Withdrawal successf         ul
                            </p>

                            <p className="mt-1 text-lg font-semibold">
                                New balance:{" "}
                                {withdrawMutation.data.balance}
                            </p>
                        </div>
                    )}

                    <button
                        type="button"
                        onClick={handleWithdraw}
                        disabled={!canWithdraw}
                        className="
                            w-full
                            rounded-2xl
                            bg-exchange-gold
                            py-3
                            font-semibold
                            text-black
                            transition
                            hover:opacity-90
                            disabled:cursor-not-allowed
                            disabled:opacit y-50
                        "
                    >
                        {withdrawMutation.isPending
                            ? "Processing..."
                            : "Withdraw"}
                    </button>
                </div>
            </div>
        </div>
    );
}