using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Transactions;

namespace Application.Common.Services;

public sealed class ExchangeService : IExchangeService
{
    public (Transaction withdrawTransaction, Transaction depositTransaction, decimal receivedAmount) Exchange(
        Wallet fromWallet,
        Wallet toWallet,
        decimal amount,
        decimal exchangeRate)
    {
        var operationId = Guid.NewGuid();

        var receivedAmount = decimal.Round(
            amount * exchangeRate,
            8,
            MidpointRounding.ToEven);

        fromWallet.Withdraw(amount);

        toWallet.Deposit(receivedAmount);

        var withdrawTransaction =
            TransactionFactory.CreateWithdraw(
                fromWallet,
                amount,
                operationId,
                exchangeRate);

        var depositTransaction =
            TransactionFactory.CreateDeposit(
                toWallet,
                receivedAmount,
                operationId,
                exchangeRate);

        return (
            withdrawTransaction,
            depositTransaction,
            receivedAmount);
    }
}
