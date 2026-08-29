using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Features.Transfer.SearchRecipients;

public sealed class SearchTransferRecipientsQueryHandler
    : IRequestHandler<SearchTransferRecipientsQuery, IReadOnlyList<TransferRecipient>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public SearchTransferRecipientsQueryHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<TransferRecipient>> Handle(
        SearchTransferRecipientsQuery request,
        CancellationToken cancellationToken)
    {
        var recipients = await _userRepository.SearchTransferRecipientsAsync(request.Query, request.Currency, cancellationToken);
        return recipients.Where(x => x.UserId != _currentUserService.UserId).ToList();
    }
}
