using MediatR;

namespace Application.Features.Exchange.ExchangeCurrency;

public sealed record ExchangeCommand(
    Guid FromWalletId,
    Guid ToWalletId,
    decimal Amount)
    : IRequest<ExchangeResponse>;

