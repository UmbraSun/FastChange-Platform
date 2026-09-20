using Core.Api.Transfers;
using Core.DTOs.Transfers;
using Core.Interfaces;

namespace Core.Services;

public sealed class TransferService : ITransferService
{
    private readonly ITransferApi _transferApi;

    public TransferService(ITransferApi transferApi)
    {
        _transferApi = transferApi;
    }

    public Task<TransferResponseDto> TransferAsync(Guid fromWalletId, Guid toWalletId, decimal amount, CancellationToken cancellationToken = default)
    {
        return _transferApi.TransferAsync(new TransferRequestDto(fromWalletId, toWalletId, amount), cancellationToken);
    }

    public Task<IReadOnlyList<TransferRecipientDto>> SearchRecipientsAsync(string query, string currency, CancellationToken cancellationToken = default)
    {
        return _transferApi.SearchRecipientsAsync(query, currency, cancellationToken);
    }
}