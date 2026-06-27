using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IWalletOperationService
{
    (Transaction transaction, decimal newBalance) Deposit(
        Wallet wallet,
        decimal amount);

    (Transaction transaction, decimal newBalance) Withdraw(
        Wallet wallet,
        decimal amount);
}
