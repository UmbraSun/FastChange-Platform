using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Contracts.Events;
using MediatR;
using Resources;
using System.Text.Json;

namespace Application.Features.Exchange.ExchangeCurrency;

public sealed class ExchangeCommandHandler
    : IRequestHandler<ExchangeCommand, ExchangeResponse>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletAccessService _walletAccessService;
    private readonly IExchangeRateProvider _exchangeRateProvider;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IOutboxRepository _outboxRepository;
    private readonly IExchangeService _exchangeService;
    private readonly IUnitOfWork _unitOfWork;

    public ExchangeCommandHandler(
        IWalletRepository walletRepository,
        IWalletAccessService walletAccessService,
        IExchangeRateProvider exchangeRateProvider,
        ITransactionRepository transactionRepository,
        IOutboxRepository outboxRepository,
        IExchangeService exchangeService,
        IUnitOfWork unitOfWork)
    {
        _walletRepository = walletRepository;
        _walletAccessService = walletAccessService;
        _exchangeRateProvider = exchangeRateProvider;
        _transactionRepository = transactionRepository;
        _outboxRepository = outboxRepository;
        _exchangeService = exchangeService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ExchangeResponse> Handle(
        ExchangeCommand request,
        CancellationToken cancellationToken)
    {
        var fromWallet = await _walletRepository.GetByIdAsync(request.FromWalletId, cancellationToken)
            ?? throw new BusinessException(Localization.WalletNotFound);

        var toWallet = await _walletRepository.GetByIdAsync(request.ToWalletId, cancellationToken)
            ?? throw new BusinessException(Localization.WalletNotFound);

        if (fromWallet.Id == toWallet.Id)
            throw new BusinessException(Localization.SourceAndDestinationWalletsMustBeDifferent);

        await _walletAccessService.EnsureAccessAsync(fromWallet, cancellationToken);
        await _walletAccessService.EnsureAccessAsync(toWallet, cancellationToken);

        var rate = await _exchangeRateProvider.GetRateAsync(
            fromWallet.Currency,
            toWallet.Currency,
            cancellationToken)
            ?? throw new BusinessException(Localization.ExchangeRateNotFound);

        var result = _exchangeService.Exchange(
            fromWallet,
            toWallet,
            request.Amount,
            rate.Rate);

        await _transactionRepository.AddAsync(
            result.WithdrawTransaction,
            cancellationToken);

        await _transactionRepository.AddAsync(
            result.DepositTransaction,
            cancellationToken);

        var integrationEvent = new ExchangeCompletedEvent(
            result.WithdrawTransaction.OperationId.Value,
            fromWallet.Id,
            toWallet.Id,
            request.Amount,
            rate.Rate,
            result.ReceivedAmount);

        await _outboxRepository.AddAsync(
            new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = typeof(ExchangeCompletedEvent).FullName!,
                Payload = JsonSerializer.Serialize(integrationEvent),
                OccurredOnUtc = DateTime.UtcNow
            },
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ExchangeResponse(
            rate.Rate,
            request.Amount,
            result.ReceivedAmount,
            fromWallet.Balance,
            toWallet.Balance);
    }
}
