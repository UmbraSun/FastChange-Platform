using Application.Common.Models;
using MediatR;

namespace Application.Features.Transfer.SearchRecipients;

public sealed record SearchTransferRecipientsQuery(
    string Query,
    string Currency)
    : IRequest<IReadOnlyList<TransferRecipient>>;
