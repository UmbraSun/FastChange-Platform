namespace Core.DTOs.Wallets;

public class DepositRequestDto
{
    public Guid WalletId { get; set; }

    public decimal Amount { get; set; }
}
