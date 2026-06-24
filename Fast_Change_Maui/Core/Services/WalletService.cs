namespace Core.Services;

public class WalletService
{
    Task DepositAsync(Guid walletId, decimal amount);
}
