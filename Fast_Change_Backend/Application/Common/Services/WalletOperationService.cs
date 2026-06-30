using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Transactions;

namespace Application.Common.Services;

public sealed class WalletOperationService : IWalletOperationService
{
    public (Transaction transaction, decimal newBalance) Deposit(
        Wallet wallet,
        decimal amount)
    {
        wallet.Deposit(amount);

        var tx = TransactionFactory.CreateDeposit(wallet, amount);

        return (tx, wallet.Balance);
    }

    public (Transaction transaction, decimal newBalance) Withdraw(
        Wallet wallet,
        decimal amount)
    {
        wallet.Withdraw(amount);

        var tx = TransactionFactory.CreateWithdraw(wallet, amount);

        return (tx, wallet.Balance);
    }

    public (Transaction withdrawTransaction, Transaction depositTransaction, decimal receivedAmount) Exchange(
        Wallet fromWallet,
        Wallet toWallet,
        decimal amount,
        decimal exchangeRate)
    {
        var receivedAmount = decimal.Round(
            amount * exchangeRate,
            8,
            MidpointRounding.ToEven);

        fromWallet.Withdraw(amount);

        toWallet.Deposit(receivedAmount);

        var operationId = Guid.NewGuid();

        var withdraw =
            TransactionFactory.CreateWithdraw(
                fromWallet,
                amount,
                operationId);

        var deposit =
            TransactionFactory.CreateDeposit(
                toWallet,
                receivedAmount,
                operationId);

        return (
            withdraw,
            deposit,
            receivedAmount);
    }
}
