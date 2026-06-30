using Domain.Entities;

namespace Application.Common.Interfaces;

/// <summary>
/// exchange service interface for handling currency exchanges between wallets. It defines a method to perform the exchange operation, which includes withdrawing from one wallet, depositing into another, and calculating the received amount based on the provided exchange rate.
/// </summary>
public interface IExchangeService
{
    /// <summary>
    /// Exchanges a specified amount from one wallet to another, applying the given exchange rate. It returns the withdraw transaction, deposit transaction, and the received amount in the target currency.
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
