using Domain.Entities;

namespace Application.Common.Interfaces;

/// <summary>
/// Wallet operation service interface for handling deposit, withdraw, and exchange operations on wallets.
/// </summary>
public interface IWalletOperationService
{
    /// <summary>
    /// Deposits a specified amount into the given wallet and returns the transaction details along with the new balance.
    /// </summary>
    /// <param name="wallet"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    (Transaction transaction, decimal newBalance) Deposit(
        Wallet wallet,
        decimal amount);

    /// <summary>
    /// Withdraws a specified amount from the given wallet and returns the transaction details along with the new balance.
    /// </summary>
    /// <param name="wallet"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    (Transaction transaction, decimal newBalance) Withdraw(
        Wallet wallet,
        decimal amount);

    /// <summary>
    /// Exchanges a specified amount from one wallet to another, applying the given exchange rate, and returns the transaction details for both wallets.
    /// </summary>
    /// <param name="fromWallet"></param>
    /// <param name="toWallet"></param>
    /// <param name="amount"></param>
    /// <param name="exchangeRate"></param>
    /// <returns></returns>
    (Transaction withdrawTransaction, Transaction depositTransaction, decimal receivedAmount) Exchange(
        Wallet fromWallet,
        Wallet toWallet,
        decimal amount,
        decimal exchangeRate);
}
