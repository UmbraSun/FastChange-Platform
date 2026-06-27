using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Common.Service;

public sealed class WalletOperationService : IWalletOperationService
{
    public (Transaction transaction, decimal newBalance) Deposit(
        Wallet wallet,
        decimal amount)
    {
        wallet.Deposit(amount);

        var tx = Transaction.Create(
            wallet,
            amount,
            amount,
            TransactionType.Deposit);

        return (tx, wallet.Balance);
    }

    public (Transaction transaction, decimal newBalance) Withdraw(
        Wallet wallet,
        decimal amount)
    {
        wallet.Withdraw(amount);

        var tx = Transaction.Create(
            wallet,
            amount,
            -amount,
            TransactionType.Withdraw);

        return (tx, wallet.Balance);
    }
}
