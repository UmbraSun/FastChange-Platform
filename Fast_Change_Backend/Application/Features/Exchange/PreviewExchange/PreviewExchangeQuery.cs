using MediatR;

namespace Application.Features.Exchange.PreviewExchange;

public sealed record PreviewExchangeQuery(
    string FromCurrency,
    string ToCurrency,
    decimal Amount)
    : IRequest<PreviewExchangeResponse>;
