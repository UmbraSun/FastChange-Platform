using Domain.Entities;
using Domain.Enums;

namespace Domain.Transactions;

public static class TransactionFactory
{
    public static Transaction CreateDeposit(
        Wallet wallet,
        decimal amount,
        Guid? operationId = null)
        => Transaction.Create(
            wallet,
            amount,
            amount,
            wallet.Balance,
            TransactionType.Deposit,
            operationId);

    public static Transaction CreateWithdraw(
        Wallet wallet, 
        decimal amount,
        Guid? operationId = null)
        => Transaction.Create(
            wallet,
            amount,
            -amount,
            wallet.Balance,
            TransactionType.Withdraw,
            operationId);
}
