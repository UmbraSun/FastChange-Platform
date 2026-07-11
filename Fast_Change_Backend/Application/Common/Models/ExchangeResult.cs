using Domain.Entities;

namespace Application.Common.Models;

public sealed record ExchangeResult(
    Transaction WithdrawTransaction,
    Transaction DepositTransaction,
    decimal ReceivedAmount);
