using MediatR;

namespace Application.Features.Exchange.PreviewExchange;

public sealed record PreviewExchangeQuery(
    Guid FromWalletId,
    Guid ToWalletId,
    decimal Amount)
    : IRequest<PreviewExchangeResponse>;
