using Domain.Entities;
using Domain.Enums;

namespace Domain.Transactions;

public static class TransactionFactory
{
    public static Transaction CreateDeposit(
        Wallet wallet,
        decimal amount,
        Guid? operationId = null,
        decimal? exchangeRate = null)
        => Transaction.Create(
            wallet,
            amount,
            amount,
            wallet.Balance,
            TransactionType.Deposit,
            operationId,
            exchangeRate);

    public static Transaction CreateWithdraw(
        Wallet wallet,
        decimal amount,
        Guid? operationId = null,
        decimal? exchangeRate = null)
        => Transaction.Create(
            wallet,
            amount,
            -amount,
            wallet.Balance,
            TransactionType.Withdraw,
            operationId,
            exchangeRate);
}
