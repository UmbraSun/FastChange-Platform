using MediatR;

namespace Application.Features.Exchange.Exchange;

public sealed record ExchangeCommand(
    Guid FromWalletId,
    Guid ToWalletId,
    decimal Amount)
    : IRequest<ExchangeResponse>;

