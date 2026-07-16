using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Features.Wallets.GetWalletHistory;

public sealed class GetWalletHistoryHandler
    : IRequestHandler<GetWalletHistoryQuery, IReadOnlyList<WalletHistoryItem>>
{
    private readonly IWalletHistoryReader _reader;

    public GetWalletHistoryHandler(IWalletHistoryReader reader)
    {
        _reader = reader;
    }

    public Task<IReadOnlyList<WalletHistoryItem>> Handle(
        GetWalletHistoryQuery query,
        CancellationToken ct)
    {
        return _reader.GetByWalletAsync(
            query.WalletId,
            query.Take,
            ct);
    }
}
